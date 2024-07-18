namespace me_academy.core.Models.View.Orders;

public class DiscountAppliedView
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal DiscountedAmount { get; set; }
    public string Message { get; set; } = null!;
}
