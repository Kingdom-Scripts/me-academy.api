namespace me_academy.core.Models.Input.SmeHub;

public class SmeHubSearchModel : PagingOptionModel
{
    public string? SearchQuery { get; set; }
    public bool? IsActive { get; set; }
    public bool WithDeleted { get; set; } = false;
}