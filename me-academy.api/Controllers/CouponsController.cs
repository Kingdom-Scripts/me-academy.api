using Azure.Core;
using me_academy.core.Interfaces;
using me_academy.core.Models.Input;
using me_academy.core.Models.Input.Coupons;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Coupons;
using me_academy.core.Models.View.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/coupons")]
[Authorize]
public class CouponsController : BaseController
{
    private readonly ICouponService _couponService;

    public CouponsController(ICouponService couponService)
        => _couponService = couponService;

    /// <summary>
    /// Checks the uniqueness of a coupon code.
    /// </summary>
    /// <remarks>
    /// This endpoint checks the uniqueness of a coupon code when creating or updating a coupon. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to exclude from the uniqueness check.</param>
    /// <param name="code">The coupon code to check for uniqueness.</param>
    /// <response code="200">Returns true if the coupon code is unique; otherwise, false.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet("{id}/unique/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> CheckCouponCodeUniqueness(int id, string code)
    {
        var result = await _couponService.CheckCouponCodeUniqueness(id, code);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Adds a new coupon.
    /// </summary>
    /// <remarks>
    /// This endpoint adds a new coupon with the specified details. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="model">The details of the coupon to add.</param>
    /// <response code="201">Returns the coupon information.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult<CouponDetailView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AddCoupon([FromBody] CouponModel model)
    {
        var result = await _couponService.AddCoupon(model);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Updates an existing coupon.
    /// </summary>
    /// <remarks>
    /// This endpoint updates an existing coupon with the specified details. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to update.</param>
    /// <param name="model">The details of the coupon to update.</param>
    /// <response code="200">Returns the updated coupon information.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpPost("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<CouponDetailView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CouponModel model)
    {
        var result = await _couponService.UpdateCoupon(id, model);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gets a coupon by identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint gets a coupon by its identifier. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to get.</param>
    /// <response code="200">Returns the coupon information.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<CouponDetailView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> GetCoupon(int id)
    {
        var result = await _couponService.GetCoupon(id);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Retrieves a list of all coupons.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all available coupons. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="request">The search parameters for filtering the list of coupons.</param>
    /// <response code="200">Returns a list of coupons.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<CouponView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> GetCoupons([FromQuery] CouponSearchModel request)
    {
        var result = await _couponService.GetCoupons(request);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Deletes a coupon by its identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint deletes a specific coupon by its identifier. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to delete.</param>
    /// <response code="200">Indicates successful deletion of the coupon.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet("{id}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> DeleteCoupon(int id)
    {
        var result = await _couponService.DeleteCoupon(id);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Activates a coupon by its identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint activates a specific coupon by its identifier. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to activate.</param>
    /// <response code="200">Indicates successful activation of the coupon.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ActivateCoupon(int id)
    {
        var result = await _couponService.ActivateCoupon(id);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Deactivates a coupon by its identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint deactivates a specific coupon by its identifier. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon to deactivate.</param>
    /// <response code="200">Indicates successful deactivation of the coupon.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [HttpGet("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> DeactivateCoupon(int id)
    {
        var result = await _couponService.DeactivateCoupon(id);
        return ProcessResponse(result);
    }


    /// <summary>
    /// Validates a discount code.
    /// </summary>
    /// <remarks>
    /// This endpoint validates a discount code and returns the discount information. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="code">The discount code to validate.</param>
    /// <param name="totalAmount">The total amount of the order.</param>
    /// <response code="200">Returns the discount information.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    [AllowAnonymous]
    [HttpGet("{code}/validate-for/{totalAmount}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<DiscountAppliedView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ValidateDiscount(string code, decimal totalAmount)
    {
        var result = await _couponService.ValidateCoupon(code, totalAmount);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gets a list of users who have used a coupon.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of users who have used a specific coupon. <br/>
    /// Requires authentication.
    /// </remarks>
    /// <param name="id">The identifier of the coupon for which to get users.</param>
    /// <param name="request">The search parameters for filtering the list of users.</param>
    /// <response code="200">Returns a list of users who have used the coupon.</response>
    /// <response code="400">Returns an error if any occurred.</response>
    /// <response code="404">Returns Not Found if the coupon does not exist.</response>
    [HttpGet("{id}/users")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<UserCouponView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundErrorResult))]
    public async Task<IActionResult> GetCouponUsers(int id, [FromQuery] PagingOptionModel request)
    {
        var result = await _couponService.GetCouponUsers(id, request);
        return ProcessResponse(result);
    }
}
