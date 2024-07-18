using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Coupons;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;
public interface ICouponService
{
    Task<Result> CheckCouponCodeUniqueness(int id, string code);
    Task<Result> AddCoupon(CouponModel model);
    Task<Result> UpdateCoupon(int id, CouponModel model);
    Task<Result> GetCoupon(int id);
    Task<Result> GetCoupons(CouponSearchModel request);
    Task<Result> DeleteCoupon(int id);
    Task<Result> ActivateCoupon(int id);
    Task<Result> DeactivateCoupon(int id);
    Task<Result> ValidateCoupon(string code, decimal totalAmount);
    Task<Result> GetCouponUsers(int id, PagingOptionModel request);
}
