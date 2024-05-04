using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iText.Layout.Element;
using me_academy.core.Interfaces;

namespace me_academy.core.Models.App;

public class Course : BaseAppModel, ISoftDeletable
{
    [Required][MaxLength(200)] public string Uid { get; set; } = null!;

    [Required][MaxLength(100)] public string Title { get; set; } = null!;

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    [Required] public bool IsDraft { get; set; } = true;

    [Required] public bool IsActive { get; set; } = true;

    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public bool IsPublished { get; set; } = false;
    public int? PublishedById { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public int ViewCount { get; set; } = 0;

    // public bool VideoIsUploaded { get; set; } = false;
    [Required] public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }

    public User? CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }
    public User? DeletedBy { get; set; }
    public CourseVideo? CourseVideo { get; set; } = new();
    public List<CourseLink> UsefulLinks { get; set; } = new();
    public List<CourseAuditLog> CourseAuditLogs { get; set; } = new();
    public List<CoursePrice> CoursePrices { get; set; } = new();
    public IQueryable<QuestionAndAnswer> QuestionAndAnswers { get; set; } = new List<QuestionAndAnswer>().AsQueryable();
}