namespace me_academy.core.Models.App;

public class UserCourse : BaseAppModel
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int Progress { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public DateTime PurchasedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

    public Course? Course { get; set; }
}