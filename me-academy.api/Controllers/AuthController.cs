using me_academy.core.Interfaces;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Authorize]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResult<AuthDataView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> SignUp(RegisterModel model)
    {
        var res = await _authService.Register(model);
        return ProcessResponse(res);
    }

    [HttpGet("request-email-confirmation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> RequestEmailConfirmation()
    {
        var res = await _authService.RequestEmailConfirmation();
        return ProcessResponse(res);
    }

    [HttpPost("confirm-email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailModel model)
    {
        var res = await _authService.ConfirmEmail(model);
        return ProcessResponse(res);
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<AuthDataView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AuthenticateUser(LoginModel model)
    {
        var res = await _authService.AuthenticateUser(model);
        return ProcessResponse(res);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<AuthDataView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
    {
        var res = await _authService.RefreshToken(model);
        return ProcessResponse(res);
    }

    [HttpPost("{userReference}/logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    public async Task<IActionResult> AuthenticateUserAsync([FromRoute] string userReference)
    {
        var res = await _authService.Logout(userReference);
        return ProcessResponse(res);
    }

    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<UserProfileView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> UserProfile()
    {
        var res = await _authService.UserProfile();
        return ProcessResponse(res);
    }
}