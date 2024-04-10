using FluentValidation;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;

namespace me_academy.core.Models.Input.Courses;

public class CourseModel
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<CoursePriceModel> Prices { get; set; } = new();
    public List<CourseLinkModel> UsefulLinks { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

public class NewCourseValidation : AbstractValidator<CourseModel>
{
    public NewCourseValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.");
        RuleFor(x => x.Prices)
            .NotEmpty().WithMessage("Prices cannot be empty.");
        RuleForEach(x => x.Prices).SetValidator(new CoursePriceValidation());

        RuleForEach(x => x.UsefulLinks).SetValidator(new CourseLinkValidator());

        RuleForEach(x => x.Tags)
            .MaximumLength(20).WithMessage("Tag cannot exceed 20 characters.");
    }
}