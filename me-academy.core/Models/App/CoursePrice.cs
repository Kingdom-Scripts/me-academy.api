using System.ComponentModel.DataAnnotations.Schema;
using me_academy.core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace me_academy.core.Models.App;

[Index(nameof(CourseId), nameof(DurationId), IsUnique = true)]
public class CoursePrice : BaseAppModel, ISoftDeletable
{
    public int CourseId { get; set; }
    public int DurationId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }

    public virtual Course? Course { get; set; }
    public virtual Duration? Duration { get; set; }
}