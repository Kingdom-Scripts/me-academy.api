using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class Course : BaseAppModel, ISoftDeletable
{
    [Required] [MaxLength(200)] public string Uid { get; set; } = null!;

    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    [Required] public bool IsDraft { get; set; } = true;

    [Required] public bool IsActive { get; set; } = true;

    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public int? PublishedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public int ViewCount { get; set; } = 0;

    [Required] public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOn { get; set; }

    public User? CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }
    public User? DeletedBy { get; set; }
    public List<CourseLink> UsefulLinks { get; set; } = new();
    public List<CourseDocument> Resources { get; set; } = new();
}