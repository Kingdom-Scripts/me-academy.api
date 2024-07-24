using FluentValidation;
using me_academy.core.Models.App.Constants;

namespace me_academy.core.Models.Input.Coupons;
public class CouponModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public decimal MinOrderAmount { get; set; }
    public int? TotalAvailable { get; set; }
    public List<string> AttachedEmails { get; set; } = new();
}

public class CouponModelValidator : AbstractValidator<CouponModel>
{
    public CouponModelValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.MinOrderAmount).GreaterThanOrEqualTo(0);
        // AttachedEmails must be a valid email address
        RuleForEach(x => x.AttachedEmails).EmailAddress();

        // Type must be equal to Percentage or Fixed
        RuleFor(x => x.Type).Must(x => x == CouponTypes.Percentage || x == CouponTypes.Fixed)
            .WithMessage($"Invalid coupon type. Only {CouponTypes.Percentage} or {CouponTypes.Fixed} is valid.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .LessThanOrEqualTo(100).When(x => x.Type == CouponTypes.Percentage)
            .WithMessage("Amount must be less than or equal to 100 when type is Percentage.");
    }
}
