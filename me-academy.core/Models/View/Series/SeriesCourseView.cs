using me_academy.core.Models.View.Courses;

namespace me_academy.core.Models.View.Series;

public class SeriesCourseView
{
    public required string SeriesUid { get; set; }
    public required string CourseUid { get; set; }
    public int Order { get; set; }

    public CourseView? Course { get; set; }
}
