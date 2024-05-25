using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using me_academy.core.Interfaces;
using me_academy.core.Models.Input.Users;
using me_academy.core.Models.View.Auth;

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

    /// <summary>
    /// Invite a user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("invites")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> InviteUser(UserInvitationModel model)
    {
        var res = await _userService.InviteUser(model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// Accept an invitation
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("invites/accept")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<AuthDataView>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> AcceptInvitation(AcceptInvitationModel model)
    {
        var res = await _userService.AcceptInvitation(model);
        return ProcessResponse(res);
    }

    /// <summary>
    /// List invited users
    /// </summary>
    /// <returns></returns>
    [HttpGet("invites")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult<List<InvitedUserView>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ListInvitedUsers()
    {
        var res = await _userService.ListInvitedUsers();
        return ProcessResponse(res);
    }

    /// <summary>
    /// Resend invitation email
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("invites/{id}/resend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResult))]
    public async Task<IActionResult> ResendInvitationEmail(int id)
    {
        var res = await _userService.ResendInvitationEmail(id);
        return ProcessResponse(res);
    }
}