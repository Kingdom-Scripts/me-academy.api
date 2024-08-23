using LazyCache;
using Mapster;
using me_academy.core.Constants;
using me_academy.core.Constants.CacheKeys;
using me_academy.core.Interfaces;
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
using Saharaviewpoint.Core.Utilities;

namespace me_academy.core.Services;

public class OrderService : IOrderService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly HttpClient _paystackClient;
    private readonly ICouponService _couponService;
    private readonly IAppCache _cache;

    public OrderService(MeAcademyContext context, UserSession userSession, IHttpClientFactory httpClientFactory, IOptions<PasystackConfig> paystackConfig, ICouponService couponService, IAppCache cache)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));

        ArgumentException.ThrowIfNullOrEmpty(nameof(paystackConfig));
        ArgumentException.ThrowIfNullOrEmpty(nameof(httpClientFactory));

        _paystackClient = httpClientFactory.CreateClient(paystackConfig.Value.HttpClientName);
        _couponService = couponService;
        _cache = cache;
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

        if (model.ItemType == ContentTypes.Course)
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

            order.ContentId = course.Id;

            // get course price
            var price = course.Prices.FirstOrDefault(x => x.DurationId == model.DurationId);
            if (price == null)
                return new ErrorResult("Invalid duration for course");

            order.ItemAmount = price.Price;
        }
        else if (model.ItemType == ContentTypes.Series)
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

            order.ContentId = series.Id;

            // get series price
            var price = series.Prices
                .FirstOrDefault(x => x.DurationId == model.DurationId);

            if (price == null)
                return new ErrorResult("Invalid duration for series");

            order.ItemAmount = price.Price;
        }
        else if (model.ItemType == ContentTypes.SmeHub)
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

            order.ContentId = smeHub.Id;
            order.ItemAmount = smeHub.Price;
        }
        else if (model.ItemType == ContentTypes.AnnotatedAgreement)
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

            order.ContentId = annotatedAgreement.Id;

            // get annotated agreement price
            order.ItemAmount = annotatedAgreement.Price;
        }
        else
        {
            return new ErrorResult("Invalid item selected.");
        }

        // attach coupon
        order.TotalAmount = order.ItemAmount;
        if (!string.IsNullOrEmpty(model.CouponCode))
        {
            var couponRes = await _couponService
                .ValidateCoupon(model.CouponCode, order.ItemAmount);
            if (couponRes is ErrorResult)
                return couponRes;

            var coupon = (DiscountAppliedView)couponRes.Content;
            order.CouponId = coupon.Id;
            order.TotalAmount = order.ItemAmount - coupon.Discount;
            order.CouponApplied = coupon.Discount;
        }

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();

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
            metadata = JsonConvert.SerializeObject(user),
            callback_url = model.CallBackUrl + $"?orderId={order.Id}"
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
        var order = await _context.Orders
            .Where(x => x.Id == orderId)
            .Include(x => x.Coupon)
            .FirstOrDefaultAsync();

        if (order is null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Order not found.");

        if (order.IsPaid) return new ErrorResult("Payment has been completed.");

        // validate coupon
        if (order!.CouponId.HasValue)
        {
            var couponRes = await _couponService.ValidateCoupon(order.Coupon!.Code, order.ItemAmount);
            if (couponRes is ErrorResult)
                return new ErrorResult("Coupon applied to order is no longer valid. Please try again.");
        }

        var paymentData = new PaymentRequestView
        {
            Authorization_Url = order.Authorization_Url!,
            Access_Code = order.Access_Code!,
            Reference = order.Reference!
        };

        return new SuccessResult(paymentData);
    }

    public async Task<Result> ConfirmPayment(int orderId)
    {
        var order = await _context.Orders
            .Where(x => x.Id == orderId)
            .Include(x => x.Duration)
            .Include(x => x.Coupon)
            .FirstOrDefaultAsync();

        if (order == null)
            return new NotFoundErrorResult("Invalid order.");

        if (string.IsNullOrEmpty(order.Reference))
            return new ErrorResult("Invalid payment confirmation attempt.");

        if (order.IsPaid)
            return new ErrorResult("Order payment is already completed.");

        var verifyTransaction = await VerifyTransaction(order.Reference!);

        if (!verifyTransaction.status)
            return new ErrorResult(verifyTransaction.message);
        if (!verifyTransaction.status || verifyTransaction.data.status != "success")
            return new ErrorResult("Payment not completed");

        var today = DateTime.UtcNow;
        order.IsPaid = true;
        order.PaidAt = today;
        order.UpdateById = _userSession.UserId;
        order.UpdatedAt = today;
        if (order.CouponId.HasValue)
        {
            order.Coupon!.TotalUsed++;
        }

        _context.Orders.Update(order);

        // Add Course progress
        if (order.ItemType == ContentTypes.Course)
        {
            var userCourse = await _context.UserCourses
                .Where(x => x.UserId == order.UserId && x.CourseId == order.ContentId)
                .FirstOrDefaultAsync();

            if (userCourse != null)
            {
                userCourse.ExpiresOnUtc = today.AddMonths(order.Duration!.Count);

                _context.UserCourses.Update(userCourse);
            }
            else
            {
                userCourse = new UserCourse
                {
                    UserId = order.UserId,
                    CourseId = order.ContentId,
                    ExpiresOnUtc = today.AddMonths(order.Duration!.Count)
                };
                await _context.AddAsync(userCourse);
            }
        }

        // Add Series progress
        else if (order.ItemType == ContentTypes.Series)
        {
            var userSeries = await _context.UserSeries
                .Where(x => x.UserId == order.UserId && x.SeriesId == order.ContentId)
                .FirstOrDefaultAsync();

            var series = await _context.Series
                .Where(x => x.Id == order.ContentId)
                .FirstOrDefaultAsync();

            if (userSeries != null)
            {
                userSeries.ExpiresOnUtc = today.AddMonths(order.Duration!.Count);

                _context.UserSeries.Update(userSeries);
            }
            else
            {
                userSeries = new UserSeries
                {
                    UserId = order.UserId,
                    SeriesId = order.ContentId,
                    ExpiresOnUtc = today.AddMonths(order.Duration!.Count)
                };
                await _context.AddAsync(userSeries);
            }

            var seriesCourses = await _context.SeriesCourses
                .Where(sc => sc.SeriesId == series.Id && !sc.IsDeleted)
                .OrderBy(sc => sc.Order)
                .Select(sc => new
                {
                    sc.CourseId,
                    sc.Order
                }).ToListAsync();

            foreach (var item in seriesCourses)
            {
                var seriesProgress = _context.SeriesProgress
                    .Where(x => x.UserSeriesId == userSeries.Id && x.CourseId == item.CourseId)
                    .FirstOrDefault();

                if (seriesProgress != null)
                {
                    seriesProgress.Order = item.Order;

                    _context.SeriesProgress.Update(seriesProgress);
                }
                else
                {
                    seriesProgress = new SeriesProgress
                    {
                        UserSeriesId = userSeries.Id,
                        CourseId = item.CourseId,
                        Order = item.Order
                    };
                    await _context.AddAsync(seriesProgress);
                }
            }
        }

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("An error occurred while saving your order details, " +
                "kindly contact the support team.");

        // clear caches
        _cache.ClearCaches(
            CouponCacheKeys.CouponUserList(),
            StatsCacheKeys.TopRegistrants()
         );

        return new SuccessResult("Payment completed successfully.");
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

        string responseString = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<PaystackResponse<VerifyTransactionResponse>>(responseString);

        return response;
    }

    #endregion
}
