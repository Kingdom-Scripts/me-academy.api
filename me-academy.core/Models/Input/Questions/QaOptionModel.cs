using FluentValidation;

namespace me_academy.core.Models.Input.Questions;

public class QaOptionModel
{
    public string Value { get; set; } = null!;
}

public class QuestionOptionValidator : AbstractValidator<QaOptionModel>
{
    public QuestionOptionValidator()
    {
        RuleFor(x => x.Value).NotEmpty().MaximumLength(255);
    }
}