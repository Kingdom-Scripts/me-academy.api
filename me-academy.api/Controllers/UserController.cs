using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using me_academy.core.Interfaces;

namespace me_academy.api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
   

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<UserProfileView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> UserProfile()
    {
        var res = await _userService.UserProfile();
        return ProcessResponse(res);
    }

}