using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace me_academy.core.Models.App;
public class ContentBase : BaseAppModel
{
    [Required][MaxLength(200)] public string Uid { get; set; }
    [Required][MaxLength(100)] public string Title { get; set; }
    [Required][MaxLength(255)] public string Summary { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Description { get; set; }
    public string Tags { get; set; }
    [Required] public bool IsActive { get; set; } = true;

    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public int ViewCount { get; set; } = 0;

    [Required] public bool IsDeleted { get; set; } = false;
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }

    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }
    public User DeletedBy { get; set; }
}
