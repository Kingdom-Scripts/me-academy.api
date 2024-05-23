using Mapster;
using me_academy.core.Constants;
using me_academy.core.Models.App;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Orders;
using me_academy.core.Models.Paystack;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Orders;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace me_academy.core.Services;

public class OrderService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly HttpClient _paystackClient;

    public OrderService(MeAcademyContext context, UserSession userSession, IHttpClientFactory httpClientFactory, IOptions<PasystackConfig> paystackConfig)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));

        ArgumentException.ThrowIfNullOrEmpty(nameof(httpClientFactory));
        _paystackClient = httpClientFactory.CreateClient(paystackConfig.Value.HttpClientName);
    }

    public async Task<Result> ValidateDiscount(string discountCode, decimal totalAmount)
    {
        if (discountCode == null)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Invalid discount code.");

        var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Code == discountCode);

        if (discount == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code not found");

        if (discount.TotalLeft == 0)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code not available.");

        if (discount.IsSingleUse)
        {
            var alreadyUsed = await _context.Orders.AnyAsync(x => x.Discount!.Code == discountCode && x.IsPaid);
            if (alreadyUsed)
                return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code already used.");
        }

        bool isValid = discount.IsActive && !discount.IsDeleted && discount.ExpiryDate > DateTime.UtcNow;
        if (!isValid)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code is not valid.");

        if (discount.MinAmount.HasValue && totalAmount < discount.MinAmount)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code is not valid for this amount.");

        var result = new DiscountView { Id = discount.Id, TotalAmount = totalAmount };

        // calculate discount
        if (discount.IsPercentage)
        {
            result.DiscountAmount = totalAmount * discount.Amount / 100;
            result.Message = $"Discount of {discount.Amount}% applied";
        }
        else
        {
            result.DiscountAmount = discount.Amount;
            result.Message = $"Discount of {discount.Amount} applied";
        }

        return new SuccessResult(result);
    }

    public async Task<Result> PlaceOrder(NewOrderModel model)
    {
        var order = new Order
        {
            BillingAddress = model.BillingAddress,
            DurationId = model.DurationId,
            ItemType = model.ItemType,
            UserId = _userSession.UserId,
        };

        if (model.ItemType == OrderItemType.Course)
        {
            var course = await _context.Courses
              .Where(x => x.Uid == model.ItemUid)
              .Select(c => new Course
              {
                  Id = c.Id,
                  Prices = c.Prices
              }).FirstOrDefaultAsync();
            if (course == null)
                return new ErrorResult("Invalid course provided");

            order.CourseId = course.Id;

            // get course price
            var price = course.Prices.FirstOrDefault(x => x.DurationId == model.DurationId);
            if (price == null)
                return new ErrorResult("Invalid duration for course");

            order.ItemAmount = price.Price;

        }
        else if (model.ItemType == OrderItemType.Series)
        {
            var series = await _context.Series
              .Where(x => x.Uid == model.ItemUid)
              .Select(s => new Series
              {
                  Id = s.Id,
                  Prices = s.Prices
              }).FirstOrDefaultAsync();
            if (series == null)
                return new ErrorResult("Invalid series provided");

            order.SeriesId = series.Id;

            // get series price
            var price = series.Prices.FirstOrDefault(x => x.DurationId == model.DurationId);
            if (price == null)
                return new ErrorResult("Invalid duration for series");

            order.ItemAmount = price.Price;
        }
        else if (model.ItemType == OrderItemType.SmeHub)
        {
            var smeHub = await _context.SmeHubs
              .Where(x => x.Uid == model.ItemUid)
              .Select(s => new SmeHub
              {
                  Id = s.Id,
                  Price = s.Price
              }).FirstOrDefaultAsync();
            if (smeHub == null)
                return new ErrorResult("Invalid sme hub provided");

            order.SmeHubId = smeHub.Id;

            order.ItemAmount = smeHub.Price;
        }
        else if (model.ItemType == OrderItemType.AnnotatedAgreement)
        {
            var annotatedAgreement = await _context.AnnotatedAgreements
              .Where(x => x.Uid == model.ItemUid)
              .Select(a => new AnnotatedAgreement
              {
                  Id = a.Id,
                  Price = a.Price
              }).FirstOrDefaultAsync();
            if (annotatedAgreement == null)
                return new ErrorResult("Invalid annotated agreement provided");

            order.AnnotatedAgreementId = annotatedAgreement.Id;

            // get annotated agreement price
            order.ItemAmount = annotatedAgreement.Price;
        }
        else
        {
            return new ErrorResult("Invalid item selected.");
        }

        // attach discount
        order.TotalAmount = order.ItemAmount;

        if (!string.IsNullOrEmpty(model.DiscountCode))
        {
            var discountRes = await ValidateDiscount(model.DiscountCode, order.ItemAmount);
            if (discountRes is ErrorResult)
                return discountRes;

            var discount = (DiscountView)discountRes.Content;
            order.DiscountId = discount.Id;
            order.TotalAmount = order.ItemAmount - discount.DiscountAmount;
            order.DiscountApplied = discount.DiscountAmount;
        }

        await _context.AddAsync(order);

        var user = await _context.Users
            .Where(u => u.Id == _userSession.UserId)
            .Select(u => new
            {
                u.Email,
                u.FirstName,
                u.LastName
            }).FirstOrDefaultAsync();

        var transactionModel = new InitiateTransactionModel
        {
            email = user!.Email,
            // convert total amount to string and remove the decimal for paystack endpoint
            amount = order.TotalAmount.ToString("F").Replace(".", ""),
            metadata = JsonConvert.SerializeObject(user)
        };

        var initiatedTransaction = await InitiateTransaction(transactionModel);
        if (!initiatedTransaction.status) return new ErrorResult(initiatedTransaction.message);

        initiatedTransaction.data.Adapt(order);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult(StatusCodes.Status201Created, order.Adapt<PaymentRequestView>())
            : new ErrorResult("An error occurred while placing the order");
    }

    public async Task<Result> AttemptPayment(int orderId)
    {
        var paymentData = await _context.Orders
            .Where(x => x.Id == orderId)
            .ProjectToType<PaymentRequestView>()
            .FirstOrDefaultAsync();

        if (paymentData == null) return new NotFoundErrorResult("Invalid order.");
        if (paymentData.IsPaid) return new ErrorResult("Payment has been completed.");

        return new SuccessResult(paymentData);
    }

    public async Task<Result> ConfirmPayment(int orderId)
    {
        var order = await _context.Orders
            .Where(x => x.Id == orderId)
            .Include(x => x.Duration)
            .FirstOrDefaultAsync();

        if (order == null) return new NotFoundErrorResult("Invalid order.");

        if (!string.IsNullOrEmpty(order.Reference)) return new ErrorResult("Invalid payment confirmation attempt.");

        if (order.IsPaid) return new ErrorResult("Order payment is already completed.");

        var verifyTransaction = await VerifyTransaction(order.Reference!);

        if (!verifyTransaction.status) return new ErrorResult(verifyTransaction.message);
        if (verifyTransaction.data.status != "success") return new ErrorResult("Payment not completed");

        order.IsPaid = true;

        var today = DateTime.UtcNow;
        var userContent = new UserContent
        {
            UserId = order.UserId,
            OrderId = order.Id,
            StartDate = today,
            EndDate = today.AddMonths(order.Duration!.Count)
        };
        await _context.AddAsync(userContent);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult("Payment completed successfully.")
            : new ErrorResult("Payment aknowleded successfully, however an issue occurred while saving your order details, kindly contact the support team.");
    }


    #region PRIVATE METHODS

    /// <summary>
    /// Initiates a transaction with the payment provider.
    /// </summary>
    private async Task<PaystackResponse<TransactionResponse>> InitiateTransaction(InitiateTransactionModel model)
    {
        StringContent jsonContent = model.ToJsonContent();

        using HttpResponseMessage httpResponse = await _paystackClient.PostAsync("/transaction/initialize", jsonContent);

        httpResponse.EnsureSuccessStatusCode();

        string responseString = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<PaystackResponse<TransactionResponse>>(responseString);

        return response;
    }

    /// <summary>
    /// Verifies a transaction with the payment provider.
    /// </summary>
    private async Task<PaystackResponse<VerifyTransactionResponse>> VerifyTransaction(string reference)
    {
        using HttpResponseMessage httpResponse = await _paystackClient.GetAsync($"/transaction/verify/{reference}");

        httpResponse.EnsureSuccessStatusCode();

        string responseString = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<PaystackResponse<VerifyTransactionResponse>>(responseString);

        return response;
    }

    #endregion
}
