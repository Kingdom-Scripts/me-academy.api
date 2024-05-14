namespace me_academy.core.Models.View.Series;

public class SeriesDetailView : SeriesView
{
    public required string Description { get; set; }
    public List<string> Tags { get; set; } = new();
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public ReferencedUserView? CreatedBy { get; set; }
    public ReferencedUserView? UpdatedBy { get; set; }
    public ReferencedUserView? DeletedBy { get; set; }
    public VideoView? Preview { get; set; } = new();
    public List<PriceView> SeriesPrices { get; set; } = new();
}