namespace me_academy.core.Models.View.Coupons;
public class CouponDetailView : CouponView
{
    public decimal Amount { get; set; }
    public int? TotalAvailable { get; set; }
    public int TotalUsed { get; set; }
    public List<string> AttachedEmails { get; set; } = new();
}
