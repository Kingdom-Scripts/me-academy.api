namespace me_academy.core.Models.View;

public class VideoView
{
    public string? VideoId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int VideoDuration { get; set; }
    public bool IsUploaded { get; set; }

    public string Duration
    {
        get
        {
            var ts = TimeSpan.FromSeconds(VideoDuration);
            return ts.ToString("hh\\:mm\\:ss");
        }
    }
}