namespace me_academy.core.Models.View.Orders;

public class DiscountView
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Message { get; set; } = null!;
}
