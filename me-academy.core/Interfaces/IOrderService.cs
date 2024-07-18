using me_academy.core.Models.Input.Orders;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IOrderService
{
    Task<Result> PlaceOrder(NewOrderModel model);
    Task<Result> AttemptPayment(int orderId);
    Task<Result> ConfirmPayment(int orderId);
}
