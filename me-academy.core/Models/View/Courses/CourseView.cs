using me_academy.core.Models.View.UtilityView;
using Newtonsoft.Json;

namespace me_academy.core.Models.View.Courses;

public class CourseView
{
    [JsonIgnore]
    public int Id { get; set; }
    public required string Uid { get; set; }
    public required   string Title { get; set; }
    public int CreatedById { get; set; }
    public required bool IsDraft { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
}