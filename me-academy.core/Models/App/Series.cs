using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class Series : ContentBase, ISoftDeletable
{

    public bool IsPublished { get; set; } = false;
    public int? PublishedById { get; set; }
    public DateTime? PublishedOnUtc { get; set; }

    public User PublishedBy { get; set; }

    public SeriesPreview Preview { get; set; }
    public List<SeriesPrice> Prices { get; set; } = new();
    public List<SeriesCourse> Courses { get; set; } = new();
    public List<UserSeries> UserSeries { get; set; } = new();
}