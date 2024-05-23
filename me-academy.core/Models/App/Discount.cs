using System.ComponentModel.DataAnnotations;

namespace me_academy.core.Models.App;

public class Discount : BaseAppModel
{
    [Required]
    [MaxLength(15)]
    public required string Code { get; set; }
    public decimal Amount { get; set; }
    public bool IsActive { get; set; }
    public bool IsPercentage { get; set; }
    public bool IsSingleUse { get; set; }
    public int TotalLeft { get; set; } = -1;
    public decimal? MinAmount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsDeleted { get; set; }

    public int CreatedById { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}
