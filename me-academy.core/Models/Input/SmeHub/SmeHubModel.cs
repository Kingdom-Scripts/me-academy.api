using FluentValidation;
using Newtonsoft.Json;

namespace me_academy.core.Models.Input.SmeHub;

public class SmeHubModel : FileUploadModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public required string Title { get; set; }
    public required string Summary{ get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class SmeHubValidation : AbstractValidator<SmeHubModel>
{
    public SmeHubValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("Summary cannot be empty.")
            .MaximumLength(200).WithMessage("Summary cannot exceed 250 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.");
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price cannot be empty.");
        RuleForEach(x => x.Tags)
            .MaximumLength(20).WithMessage("Tag cannot exceed 20 characters.");
    }
}