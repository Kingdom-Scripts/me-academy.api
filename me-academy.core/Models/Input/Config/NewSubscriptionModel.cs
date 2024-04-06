using FluentValidation;

namespace me_academy.core.Models.Input.Config;

public class NewSubscriptionModel
{
    public int Quantity { get; set; }
    public required string DurationType { get; set; }
}

public class NewSubscriptionValidation : AbstractValidator<NewSubscriptionModel>
{
    private readonly List<string> _acceptableDuration = new List<string>
    {
        "Days", "Weeks", "Months", "Years"
    };

    public NewSubscriptionValidation()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Please provide a valid quantity");
        RuleFor(x => x.DurationType)
            .NotNull().WithMessage("Please provide a duration type.")
            .Must(value => _acceptableDuration.Contains(value))
            .WithMessage($"Only {string.Join(", ", _acceptableDuration)} are acceptable values.");
    }
}