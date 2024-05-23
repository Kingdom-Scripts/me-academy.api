using me_academy.core.Constants;
using me_academy.core.Models.App;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Orders;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;

public class OrderService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;

    public OrderService(MeAcademyContext context, UserSession userSession)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
    }

    public async Task<Result> ValidateDiscount(string discount, decimal totalAmount)
    {
        if (discount == null)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Invalid discount code.");

        var discountCode = await _context.Discounts.FirstOrDefaultAsync(x => x.Code == discount);

        if (discountCode == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code not found");

        if (discountCode.TotalLeft == 0)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code not available.");

        if (discountCode.IsSingleUse)
        {
            var alreadyUsed = await _context.Orders.AnyAsync(x => x.Discount!.Code == discount && x.IsPaid);
            if (alreadyUsed)
                return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code already used.");
        }

        bool isValid = discountCode.IsActive && !discountCode.IsDeleted && discountCode.ExpiryDate > DateTime.UtcNow;
        if (!isValid)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code is not valid.");

        if (discountCode.MinAmount.HasValue && totalAmount < discountCode.MinAmount)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code is not valid for this amount.");

        var result = new DiscountView { TotalAmount = totalAmount };

        // calculate discount
        if (discountCode.IsPercentage)
        {
            result.DiscountAmount = totalAmount * discountCode.Amount / 100;
            result.Message = $"Discount of {discountCode.Amount}% applied";
        }
        else
        {
            result.DiscountAmount = discountCode.Amount;
            result.Message = $"Discount of {discountCode.Amount} applied";
        }

        return new SuccessResult(result);
    }

    public async Task<Result> PlaceOrder(NewOrderModel model)
    {
        // validate duration
        var duration = await _context.Durations.FirstOrDefaultAsync(x => x.Id == model.DurationId);        

        if (duration == null)
            return new ErrorResult("Invalid duration provided");

        var order = new Order
        {
            BillingAddress = model.BillingAddress,
            DurationId = model.DurationId,
            ItemType = model.ItemType,
            UserId = _userSession.UserId,
        };

        if (model.ItemType == OrderItemType.Course) {
          var course = await  _context.Courses
            .Where(x => x.Uid == model.ItemUid)
            .Select(c => new Course {
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
      else if (model.ItemType == OrderItemType.Series) {
          var series = await  _context.Series
            .Where(x => x.Uid == model.ItemUid)
            .Select(s => new Series {
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
      else if (model.ItemType == OrderItemType.SmeHub) {
          var smeHub = await  _context.SmeHubs
            .Where(x => x.Uid == model.ItemUid)
            .Select(s => new SmeHub {
              Id = s.Id,
              Prices = s.Prices
            }).FirstOrDefaultAsync();
          if (smeHub == null)
            return new ErrorResult("Invalid sme hub provided");

          order.SmeHubId = smeHub.Id;

          // get sme hub price
          var price = smeHub.Prices.FirstOrDefault(x => x.DurationId == model.DurationId);
          if (price == null)
            return new ErrorResult("Invalid duration for sme hub");

          order.ItemAmount = price.Price;
      }
      else if (model.ItemType == OrderItemType.AnnotatedAgreement) {
          var annotatedAgreement = await  _context.AnnotatedAgreements
            .Where(x => x.Uid == model.ItemUid)
            .Select(a => new AnnotatedAgreement {
              Id = a.Id,
              Prices = a.Prices
            }).FirstOrDefaultAsync();
          if (annotatedAgreement == null)
            return new ErrorResult("Invalid annotated agreement provided");

          order.AnnotatedAgreementId = annotatedAgreement.Id;

          // get annotated agreement price
          var price = annotatedAgreement.Prices.FirstOrDefault(x => x.DurationId == model.DurationId);
          if (price == null)
            return new ErrorResult("Invalid duration for annotated agreement");

          order.ItemAmount = price.Price;
      }

        
        // attach discount
        order = await AttachDiscount(order, model.DiscountCode);


    }

    private async Task<Order> AttachDiscount(Order order, string? code)  {
        order.TotalAmount = order.ItemAmount;

      if (string.IsNullOrEmpty(code))
        return order;

      var discountRes = await ValidateDiscount(code, order.ItemAmount);
      if (discountRes is ErrorResult)
          throw new Exception("Invalid discount code");
      
      var discount = (DiscountView)discountRes.Content;
      order.TotalAmount = order.ItemAmount - discount.DiscountAmount;
      order.DiscountApplied = discount.DiscountAmount;
      // order.DiscountCode = code;

      return order;      
    }
}
