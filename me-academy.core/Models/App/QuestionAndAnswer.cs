using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class QuestionAndAnswer : BaseAppModel
{
    public int CourseId { get; set; }
    [Required] [MaxLength(500)] public string Text { get; set; } = null!;
    public bool IsMultiple { get; set; }
    public bool IsRequired { get; set; }

    public int CreatedById { get; set; }

    public Course? Course { get; set; }
    public User? CreatedBy { get; set; }
    public ICollection<QaOption> Options { get; set; } = new List<QaOption>();
}