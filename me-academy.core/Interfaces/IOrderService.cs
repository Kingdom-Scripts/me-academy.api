using me_academy.core.Models.Input.Orders;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IOrderService
{
    Task<Result> ValidateDiscount(string discountCode, decimal totalAmount);
    Task<Result> PlaceOrder(NewOrderModel model);
    Task<Result> AttemptPayment(int orderId);
    Task<Result> ConfirmPayment(int orderId);
}
