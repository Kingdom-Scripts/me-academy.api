namespace me_academy.core.Models.App;

public class CourseDocument
{
    public int CourseId { get; set; }
    public int DocumentId { get; set; }

    public Course? Course { get; set; }
    public Document? Document { get; set; }
}