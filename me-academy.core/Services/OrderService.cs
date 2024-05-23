using me_academy.core.Models.App;
using me_academy.core.Models.Input.Orders;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Services;

public class OrderService
{
    private readonly MeAcademyContext _context;

    public OrderService(MeAcademyContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result> ValidateDiscount(string discount, decimal totalAmount)
    {
        if (discount == null)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Invalid disconunt code.");

        var discountCode = await _context.Discounts.FirstOrDefaultAsync(x => x.Code == discount);

        if (discountCode == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code not found");

        if (discountCode.Available == 0)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code not available.");

        if (discountCode.IsSingleUse)
        {
            var alreadyUsed = await _context.Orders.AnyAsync(x => x.DiscountCode == discount && x.IsPaid);
            if (alreadyUsed)
                return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code already used.");
        }

        bool isValid = discountCode.IsActive && !discountCode.IsDeleted && discountCode.ExpiryDate > DateTime.UtcNow;
        if (!isValid)
            return new ErrorResult(StatusCodes.Status404NotFound, "Discount code is not valid.");

        if (discountCode.MinAmount.HasValue && totalAmount < discountCode.MinAmount)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Discount code is not valid for this amount.");

        var result = new DiscountView();



        return new SuccessResult();
    }

    public async Task<Result> PlaceOrder(NewOrderModel model)
    {
        // validate duration
        var duration = await _context.Durations.FirstOrDefaultAsync(x => x.Id == model.DurationId);

        if (duration == null)
            return new ErrorResult("Invalid duration provided");

        // validate discount
       if (!string.IsNullOrEmpty(model.DiscountCode))
        {
            var discountRes = await ValidateDiscount(model.DiscountCode, duration
        }


    }
}
