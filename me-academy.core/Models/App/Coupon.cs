using me_academy.core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace me_academy.core.Models.App;  

public class Coupon : BaseAppModel, ISoftDeletable
{
    [Required]
    [MaxLength(255)]
    public string Code { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public bool IsActive { get; set; } = true;

    public string Type { get; set; }

    public int? TotalAvailable { get; set; }

    public int TotalUsed { get; set; } = 0;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MinOrderAmount { get; set; } = 0m;

    public DateTime? ExpiryDate { get; set; }

    public bool IsDeleted { get; set; } = false;

    public int CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public int? DeletedById { get; set; }

    public DateTime? DeletedOnUtc { get; set; }

    public string AttachedEmails { get; set; }


    public List<Order> Orders { get; set; } = new();
}
