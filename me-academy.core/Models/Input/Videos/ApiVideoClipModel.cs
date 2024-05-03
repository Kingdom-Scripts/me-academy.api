using FluentValidation;

namespace me_academy.core.Models.Input.Videos;

public class ApiVideoClipModel
{
    public string startTimeCode { get; set; } = null!;
    public string endTimeCode { get; set; } = null!;
}

public class ApiVideoClipValidator : AbstractValidator<ApiVideoClipModel>
{
    public ApiVideoClipValidator()
    {
        RuleFor(x => x.startTimeCode).NotEmpty();

        RuleFor(x => x.endTimeCode).NotEmpty();
    }
}