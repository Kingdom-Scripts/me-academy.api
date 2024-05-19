using FluentValidation;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;

namespace me_academy.core.Models.Input;

public class FileUploadModel
{
    public required IFormFile File { get; set; }
}

public class FileUploadValidator : AbstractValidator<FileUploadModel>
{
    public FileUploadValidator()
    {
        RuleFor(x => x.File)
            .Custom((file, context) =>
            {
                var validationResult = CustomFileValidator.HaveValidFile(file);
                if (!validationResult.IsValid)
                    context.AddFailure($"File: {validationResult.ErrorMessage}");
            });
    }
}