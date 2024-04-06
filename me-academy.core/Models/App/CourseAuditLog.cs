using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class CourseAuditLog : BaseAppModel
{
    public int CourseId { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }
    public int CreatedById { get; set; }

    public Course? Course { get; set; }
    public User? CreatedBy { get; set; }
}