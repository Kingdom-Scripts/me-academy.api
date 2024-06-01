using Newtonsoft.Json;

namespace me_academy.core.Models.View.Series;

public class SeriesView
{
    [JsonIgnore]
    public int Id { get; set; }
    public required string Uid { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public int CreatedById { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public VideoView? Preview { get; set; }
}