namespace me_academy.core.Models.Input.Series;

public class SeriesSearchModel : PagingOptionModel
{
    public string? SearchQuery { get; set; }
    public bool IsActive { get; set; }
    public bool WithDeleted { get; set; }
}