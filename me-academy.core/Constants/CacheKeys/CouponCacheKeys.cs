namespace me_academy.core.Constants.CacheKeys;
internal static class CouponCacheKeys
{
    public static string CouponExist(string code) => $"CouponService-CouponCodeExist-{code}";
    public static string CouponList() => "CouponService-CouponList";
    public static string CouponDetail(int id) => $"CouponService-CouponDetail-{id}";
    public static string CouponDetailByCode(string param) => $"CouponService-CouponDetailByCode-{param}";
    public static string CouponUserList() => $"CouponService-CouponUsers";
    public static string CouponUsers(string param) => $"CouponService-CouponUsers-{param}";
}
