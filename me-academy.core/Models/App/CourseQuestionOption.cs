using System.ComponentModel.DataAnnotations;
using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class CourseQuestionOption : BaseAppModel, ISoftDeletable
{
    public int QuestionId { get; set; }
    [Required] [MaxLength(255)] public string Value { get; set; } = null!;

    public CourseQuestion? Question { get; set; }

    public bool IsDeleted { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}