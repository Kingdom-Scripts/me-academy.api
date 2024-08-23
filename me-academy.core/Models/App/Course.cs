using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class Course : ContentBase, ISoftDeletable
{
    public bool IsPublished { get; set; } = false;
    public int? PublishedById { get; set; }
    public DateTime? PublishedOnUtc { get; set; }

    public bool ForSeriesOnly { get; set; } = false;

    public CourseVideo Video { get; set; }
    public List<CourseLink> UsefulLinks { get; set; } = new();
    public List<ContentLog> AuditLogs { get; set; } = new();
    public List<CoursePrice> Prices { get; set; } = new();
    public IQueryable<CourseQuestion> QuestionAndAnswers { get; set; } = new List<CourseQuestion>().AsQueryable();
    public List<SeriesCourse> SeriesCourses { get; set; } = new();
    public List<CourseDocument> Resources { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<UserCourse> UserCourses { get; set; } = new();
}