namespace me_academy.core.Models.View.Orders;

public class DiscountView
{
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public required string Message { get; set; }
}
