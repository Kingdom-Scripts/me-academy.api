using LazyCache;
using Mapster;
using me_academy.core.Constants.CacheKeys;
using me_academy.core.Extensions;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Input.Coupons;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Coupons;
using me_academy.core.Models.View.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Saharaviewpoint.Core.Utilities;

namespace me_academy.core.Services;
internal class CouponService : ICouponService
{
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;
    private readonly IAppCache _cache;

    public CouponService(MeAcademyContext context, UserSession userSession, IAppCache cache)
    {
        _context = context;
        _userSession = userSession;
        _cache = cache;
    }

    public async Task<Result> CheckCouponCodeUniqueness(int id, string code)
    {
        string cacheKey = CacheUtil.GenerateCacheKey(id, code);
        var result = await _cache
            .GetOrAddAsync(CouponCacheKeys.CouponExist(cacheKey), async () =>
        {
            var couponExist = await _context.Coupons
                .AnyAsync(c => c.Code == code && c.Id != id);

            return couponExist;
        }, new TimeSpan(0, 45, 0));

        return new SuccessResult(content: result);
    }

    public async Task<Result> AddCoupon(CouponModel model)
    {
        var couponExist = await CheckCouponCodeUniqueness(model.Id, model.Code);
        if (couponExist.Success && (bool)couponExist.Content)
            return new ErrorResult("Coupon code already exist.");

        Coupon coupon = new()
        {
            Code = model.Code,
            Amount = model.Amount,
            Type = model.Type,
            TotalAvailable = model.TotalAvailable,
            MinOrderAmount = model.MinOrderAmount,
            ExpiryDate = model.ExpiryDate,
            CreatedById = _userSession.UserId,
            AttachedEmails = model.AttachedEmails.Any()
                ? string.Join(", ", model.AttachedEmails)
                : null
        };

        await _context.Coupons.AddAsync(coupon);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to save coupon.");

        _cache.ClearCaches(CouponCacheKeys.CouponList());

        return new SuccessResult(StatusCodes.Status201Created, coupon.Adapt<CouponDetailView>());
    }

    public async Task<Result> UpdateCoupon(int id, CouponModel model)
    {
        Coupon coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon == null)
            return new ErrorResult("Coupon not found.");

        var couponExist = await CheckCouponCodeUniqueness(model.Id, model.Code);
        if (couponExist.Success && (bool)couponExist.Content)
            return new ErrorResult("Coupon code already exist.");

        if (model.TotalAvailable.HasValue && model.TotalAvailable < coupon.TotalUsed)
            return new ErrorResult("Total available cannot be less than total used.");

        coupon.Code = model.Code;
        coupon.Amount = model.Amount;
        coupon.Type = model.Type;
        coupon.TotalAvailable = model.TotalAvailable;
        coupon.MinOrderAmount = model.MinOrderAmount;
        coupon.ExpiryDate = model.ExpiryDate;
        coupon.AttachedEmails = string.Join(", ", model.AttachedEmails);
        coupon.UpdatedById = _userSession.UserId;
        coupon.UpdatedOnUtc = DateTime.UtcNow;

        _context.Coupons.Update(coupon);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to update coupon.");

        _cache.ClearCaches(CouponCacheKeys.CouponList(),
               CouponCacheKeys.CouponDetail(model.Id),
               CouponCacheKeys.CouponDetailByCode(model.Code));

        return new SuccessResult(coupon.Adapt<CouponDetailView>());
    }

    public async Task<Result> GetCoupon(int id)
    {
        string cacheKey = CouponCacheKeys.CouponDetail(id);

        var coupon = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var couponDetail = await _context.Coupons
                .Where(c => c.Id == id && !c.IsDeleted)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Type,
                    c.Amount,
                    c.MinOrderAmount,
                    c.TotalAvailable,
                    c.TotalUsed,
                    c.ExpiryDate,
                    c.IsActive,
                    c.AttachedEmails,
                    TotalAmountIncured = _context.Orders
                        .Where(or => or.CouponId == c.Id).Sum(or => or.CouponApplied)
                })
                .FirstOrDefaultAsync();

            if (couponDetail == null)
                return null;

            return new CouponDetailView
            {
                Id = couponDetail.Id,
                Code = couponDetail.Code,
                Type = couponDetail.Type,
                Description = couponDetail.Type == CouponTypes.Percentage
                    ? $"{couponDetail.Amount}% off"
                    : $"₦{couponDetail.Amount} off",
                MinOrderAmount = couponDetail.MinOrderAmount,
                Usage = couponDetail.TotalAvailable.HasValue && couponDetail.TotalAvailable.Value > 0
                    ? $"{couponDetail.TotalUsed} of {couponDetail.TotalAvailable} used"
                    : $"{couponDetail.TotalUsed} of Unlimited used",
                TotalAmountIncured = couponDetail.TotalAmountIncured,
                ExpiryDate = couponDetail.ExpiryDate,
                IsActive = couponDetail.IsActive,
                Amount = couponDetail.Amount,
                TotalAvailable = couponDetail.TotalAvailable,
                TotalUsed = couponDetail.TotalUsed,
                AttachedEmails = couponDetail.AttachedEmails.Split(new[] { ", " }, StringSplitOptions.None).ToList()
            };
        }, new TimeSpan(0, 45, 0));

        if (coupon == null)
            return new ErrorResult("Coupon not found.");

        return new SuccessResult(coupon);
    }

    public async Task<Result> GetCoupons(CouponSearchModel request)
    {
        string cacheKey = CouponCacheKeys.CouponList();

        var coupons = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var couponList = await _context.Coupons
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Where(c => !request.IsActive.HasValue || c.IsActive == request.IsActive)
                .Where(c => string.IsNullOrEmpty(request.SearchQuery) || c.Code.Contains(request.SearchQuery))
                // filter by usage percentage
                .Where(c => !request.UsagePercentage.HasValue
                    || !c.TotalAvailable.HasValue || c.TotalAvailable.Value <= 0
                    || (c.TotalUsed * 100 / c.TotalAvailable.Value) <= request.UsagePercentage)
                // filter by still active by
                .Where(c => !request.StillActiveBy.HasValue
                                   || !c.ExpiryDate.HasValue
                                   || c.ExpiryDate.Value > request.StillActiveBy.Value)
                .OrderByDescending(c => c.CreatedAtUtc).ThenBy(c => c.IsActive)
                .Select(c => new CouponView
                {
                    Id = c.Id,
                    Code = c.Code,
                    Type = c.Type,
                    Description = c.Type == CouponTypes.Percentage
                        ? $"{c.Amount}% off"
                        : $"₦{c.Amount} off",
                    MinOrderAmount = c.MinOrderAmount,
                    Usage = c.TotalAvailable.HasValue && c.TotalAvailable.Value > 0
                        ? $"{c.TotalUsed} of {c.TotalAvailable} used"
                        : $"{c.TotalUsed} of Unlimited used",
                    TotalAmountIncured = c.Orders.Sum(or => or.CouponApplied),
                    ExpiryDate = c.ExpiryDate,
                    IsActive = c.IsActive
                })
                .ToPaginatedListAsync(request.PageIndex, request.PageSize);

            return couponList;
        }, new TimeSpan(0, 45, 0));

        return new SuccessResult(coupons);
    }

    public async Task<Result> DeleteCoupon(int id)
    {
        Coupon coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon == null)
            return new ErrorResult("Coupon not found.");

        _context.Coupons.Remove(coupon);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to delete coupon.");

        _cache.ClearCaches(CouponCacheKeys.CouponList(),
                          CouponCacheKeys.CouponDetail(id),
                          CouponCacheKeys.CouponDetailByCode(coupon.Code));

        return new SuccessResult("Coupon deleted successfully.");
    }

    public async Task<Result> ActivateCoupon(int id)
    {
        Coupon coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon == null)
            return new ErrorResult("Coupon not found.");

        // if coupon is expired
        if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < DateTime.UtcNow)
            return new ErrorResult("Coupon has expired.");

        coupon.IsActive = true;
        coupon.UpdatedById = _userSession.UserId;
        coupon.UpdatedOnUtc = DateTime.UtcNow;

        _context.Coupons.Update(coupon);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to activate coupon.");

        _cache.ClearCaches(CouponCacheKeys.CouponList(),
                CouponCacheKeys.CouponDetail(id),
                CouponCacheKeys.CouponDetailByCode(coupon.Code));

        return new SuccessResult("Coupon activated successfully.");
    }

    public async Task<Result> DeactivateCoupon(int id)
    {
        Coupon coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon == null)
            return new ErrorResult("Coupon not found.");

        coupon.IsActive = false;
        coupon.UpdatedById = _userSession.UserId;
        coupon.UpdatedOnUtc = DateTime.UtcNow;

        _context.Coupons.Update(coupon);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Failed to deactivate coupon.");

        _cache.ClearCaches(CouponCacheKeys.CouponList(),
                CouponCacheKeys.CouponDetail(id),
                CouponCacheKeys.CouponDetailByCode(coupon.Code));

        return new SuccessResult("Coupon deactivated successfully.");
    }

    public async Task<Result> ValidateCoupon(string code, decimal totalAmount)
    {
        string cacheKey = CouponCacheKeys.CouponDetailByCode(code);

        var coupon = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(d => d.Code.Trim() == code.Trim());

            return coupon;
        }, new TimeSpan(0, 45, 0));

        if (coupon == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "Coupon code not found");

        if (coupon.TotalAvailable.HasValue && coupon.TotalUsed > coupon.TotalAvailable)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Coupon code not available.");

        bool isValid = coupon.IsActive && !coupon.IsDeleted && coupon.ExpiryDate > DateTime.UtcNow;
        if (!isValid)
            return new ErrorResult(StatusCodes.Status404NotFound, "Coupon code is not valid.");

        // get user logged
        string loggedInUserEmail = await _context.Users
            .Where(u => u.Id == _userSession.UserId)
            .Select(u => u.Email)
            .FirstAsync();
        if (coupon.AttachedEmails != null && !coupon.AttachedEmails.Split(new[] { ", " }, StringSplitOptions.None).Contains(loggedInUserEmail))
            return new ErrorResult(StatusCodes.Status400BadRequest, "Coupon code is not valid.");

        if (totalAmount < coupon.MinOrderAmount)
            return new ErrorResult(StatusCodes.Status400BadRequest, "Coupon code is not valid for this amount.");

        var result = new DiscountAppliedView { Id = coupon.Id, TotalAmount = totalAmount };

        // calculate discount
        if (coupon.Type == CouponTypes.Percentage)
        {
            result.Discount = totalAmount * coupon.Amount / 100;
            result.DiscountedAmount = totalAmount - result.Discount;
            result.Message = $"Discount of {coupon.Amount}% applied, you have saved {result.Discount}";
        }
        else
        {
            result.Discount = coupon.Amount;
            result.DiscountedAmount = totalAmount - result.Discount;
            result.Message = $"Discount applied, you have saved {result.Discount}";
        }

        return new SuccessResult(result);
    }

    public async Task<Result> GetCouponUsers(int id, PagingOptionModel request)
    {
        string cacheKey = CacheUtil.GenerateCacheKey(request, id);

        // Retrieve the current list of cache keys and add the new key
        var cacheKeys = _cache.GetOrAdd(CouponCacheKeys.CouponUserList(), () => new List<string>(), new TimeSpan(0, 45, 0));
        if (!cacheKeys.Contains(cacheKey))
        {
            cacheKeys.Add(cacheKey);
            _cache.Add(CouponCacheKeys.CouponUserList(), cacheKeys);
        }

        // Try to get the cached result
        var cachedResult = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            return _context.Orders
                .Where(o => o.CouponId == id && o.IsPaid)
                .OrderByDescending(o => o.CreatedAtUtc)
                .Select((o, index) => new UserCouponView
                {
                    Index = index + 1,
                    Name = o.UserContent.User.FirstName + " " + o.UserContent.User.LastName,
                    Email = o.UserContent.User.Email,
                    TotalCost = o.TotalAmount,
                    DiscountApplied = o.CouponApplied,
                    DatePurchased = o.PaidAt.Value
                })
                .ToPaginatedListAsync(request.PageIndex, request.PageSize);

        }, new TimeSpan(0, 45, 0));

        return new SuccessResult(cachedResult);
    }
}