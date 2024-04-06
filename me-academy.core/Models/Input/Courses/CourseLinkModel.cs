using FluentValidation;

namespace me_academy.core.Models.Input.Courses;

public class CourseLinkModel
{
    public required string Title { get; set; }
    public required string Link { get; set; }
}

public class CourseLinkValidator : AbstractValidator<CourseLinkModel>
{
    public CourseLinkValidator()
    {
        RuleFor(model => model.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

        RuleFor(model => model.Link)
            .NotEmpty().WithMessage("Link cannot be empty.")
            .MaximumLength(255).WithMessage("Link cannot exceed 255 characters.");
    }
}