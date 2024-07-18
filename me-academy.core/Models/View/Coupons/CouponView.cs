namespace me_academy.core.Models.View.Coupons;
public class CouponView
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Type { get; set; } 
    public string Description { get; set; }
    public decimal MinOrderAmount { get; set; }
    public string Usage { get; set; }
    public decimal TotalAmountIncured { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}
