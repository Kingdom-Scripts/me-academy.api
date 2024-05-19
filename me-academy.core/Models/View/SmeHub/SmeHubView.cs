using me_academy.core.Models.Utilities;

namespace me_academy.core.Models.View.SmeHub;

public class SmeHubView
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; }
}
