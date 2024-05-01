using FluentValidation;

namespace me_academy.core.Models.Input.Videos;

public class VideoDetailModel
{
    public string VideoId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? PlayerId { get; set; }
    public bool Mp4Support { get; set; } = false;
}

public class VideoDetailValidator : AbstractValidator<VideoDetailModel>
{
    public VideoDetailValidator()
    {
        RuleFor(x => x.VideoId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}