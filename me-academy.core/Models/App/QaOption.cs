using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class QaOption : BaseAppModel
{
    public int QaId { get; set; }
    [Required] [MaxLength(255)] public string Value { get; set; } = null!;

    public QuestionAndAnswer? Qa { get; set; }
}