using System.ComponentModel.DataAnnotations;
using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class SeriesQuestionOption : BaseAppModel, ISoftDeletable
{
    public int QuestionId { get; set; }
    [Required][MaxLength(255)] public string Value { get; set; } = null!;

    public int CreatedById { get; set; }

    public int? UpdatedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    
    public bool IsDeleted { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }

    public SeriesQuestion? Question { get; set; }
}