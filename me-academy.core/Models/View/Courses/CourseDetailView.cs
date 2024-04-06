using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.UtilityView;

namespace me_academy.core.Models.View.Courses;

public class CourseDetailView : CourseView
{
    public required string Description { get; set; }
    public List<string> Tags { get; set; } = new();
    public int? UpdatedById { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public List<CourseLinkView> UsefulLinks { get; set; } = new();
    public List<DocumentView> Resources { get; set; } = new();

    public ReferencedUserView? CreatedBy { get; set; }
    public ReferencedUserView? UpdatedBy { get; set; }
    public ReferencedUserView? DeletedBy { get; set; }
}