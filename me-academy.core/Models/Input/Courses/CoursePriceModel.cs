using FluentValidation;

namespace me_academy.core.Models.Input.Courses;

public class CoursePriceModel
{
    public int DurationId { get; set; }
    public decimal Price { get; set; }
}

public class CoursePriceValidation : AbstractValidator<CoursePriceModel>
{
    public CoursePriceValidation()
    {
        RuleFor(x => x.DurationId)
            .NotEmpty().WithMessage("Duration Id cannot be empty.");
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price cannot be empty.");
    }
}