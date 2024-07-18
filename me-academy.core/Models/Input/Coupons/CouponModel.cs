using FluentValidation;

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
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.MinOrderAmount).GreaterThanOrEqualTo(0);

        // Type must be equal to Percentage or Fixed
        RuleFor(x => x.Type).Must(x => x == "Percentage" || x == "Fixed")
            .WithMessage("Invalid coupon type.");

        // AttachedEmails must be a valid email address
        RuleForEach(x => x.AttachedEmails).EmailAddress();
    }
}
