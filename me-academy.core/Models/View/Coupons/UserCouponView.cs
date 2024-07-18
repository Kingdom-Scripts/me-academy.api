namespace me_academy.core.Models.View.Coupons;
public class UserCouponView
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal TotalCost { get; set; }
    public decimal DiscountApplied { get; set; }
    public DateTime DatePurchased { get; set; }
}
