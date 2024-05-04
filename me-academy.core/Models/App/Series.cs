using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace me_academy.core.Models.App;

public class Series : BaseAppModel
{
    [Required][MaxLength(200)] public string Uid { get; set; } = null!;
    [Required][MaxLength(100)] public string Title { get; set; } = null!;

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; } = null!;
    [StringLength(255)]
    public string Summary { get; set; } = null!;
    [Required] public bool IsDraft { get; set; } = true;
    [Required] public bool IsActive { get; set; } = true;
    public string? Tags { get; set; }

    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public int? PublishedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public int ViewCount { get; set; } = 0;

    [Required] public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }

    public User? CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }
    public User? PublishedBy { get; set; }
    public User? DeletedBy { get; set; }
}
