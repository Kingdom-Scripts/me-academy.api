using FluentValidation;

namespace me_academy.core.Models.Input.Questions;

public class QuestionAndAnswerModel
{
    public string Text { get; set; } = null!;
    public bool IsMultiple { get; set; }
    public bool IsRequired { get; set; }

    public List<QaOptionModel> Options { get; set; } = new();
}

public class QuestionValidator : AbstractValidator<QuestionAndAnswerModel>
{
    public QuestionValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        RuleForEach(x => x.Options).SetValidator(new QuestionOptionValidator());
    }
}