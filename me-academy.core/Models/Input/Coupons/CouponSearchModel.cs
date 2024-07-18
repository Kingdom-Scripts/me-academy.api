namespace me_academy.core.Models.Input.Coupons;
public class CouponSearchModel : PagingOptionModel
{
    public bool? IsActive { get; set; }
    public int? UsagePercentage { get; set; }
    public DateTime? StillActiveBy { get; set; }
}
