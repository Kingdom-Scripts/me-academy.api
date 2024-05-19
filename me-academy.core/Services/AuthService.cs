using Mapster;
using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Utilities;
using me_academy.core.Models.View.Auth;
using me_academy.core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Web;

namespace me_academy.core.Services;

public class AuthService : IAuthService
{
    private readonly MeAcademyContext _context;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserSession _userSession;
    private readonly IEmailService _emailService;
    private readonly BaseURLs _baseUrls;

    public AuthService(MeAcademyContext context, ITokenGenerator tokenGenerator, UserSession userSession,
        IEmailService emailService, IOptions<AppConfig> options)
    {
        if (options is null) throw new ArgumentNullException(nameof(options));

        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

        _baseUrls = options.Value.BaseURLs;
    }

    public async Task<Result> Register(RegisterModel model)
    {
        // validate user with email doesn't exist
        bool userExist = await _context.Users
            .AnyAsync(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim());

        if (userExist)
            return new ErrorResult("An account with this email already exist. Please log in instead.");

        // create user object
        var user = model.Adapt<User>();
        user.HashedPassword = model.Password.HashPassword();

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == (int)Roles.Customer);
        var userRole = new UserRole { User = user, Role = role };

        // save user
        await _context.AddAsync(user);
        await _context.AddAsync(userRole);

        int saved = await _context.SaveChangesAsync();
        if (saved < 1)
            return new ErrorResult("Unable to add user at the moment. Please try again");

        // send confirmation email
        string token = CodeGenerator.GenerateCode(100);
        await SaveNewCode(user.Id, token, CodePurposes.ConfirmEmail);
        await _emailService.SendConfirmEmail(user.Email, token);

        // create user token
        user.UserRoles = new List<UserRole> { userRole };
        var authData = await _tokenGenerator.GenerateJwtToken(user);

        // return user token
        if (!authData.Success)
            return new ErrorResult(authData.Message);

        return new SuccessResult(StatusCodes.Status201Created, authData.Content);
    }

    public async Task<Result> ConfirmEmail(ConfirmEmailModel model)
    {
        // validate request
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim());

        if (user == null)
            return new ErrorResult("Invalid email address.");

        if (user.EmailConfirmed)
            return new ErrorResult("Email already verified.");

        // validate token
        var today = DateTime.UtcNow;
        var code = await _context.Codes
            .FirstOrDefaultAsync(c => c.OwnerId == user.Id
                                      && c.Purpose == CodePurposes.ConfirmEmail
                                      && c.Token == model.Token
                                      && c.ExpiryDate > today
                                      && c.Used == false);
        if (code == null)
            return new ErrorResult("Invalid request, kindly request a new confirmation email.");

        // update user and token
        user.EmailConfirmed = true;
        code.Used = true;

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult("Email verified successfully.")
            : new ErrorResult("Unable to verify email at the moment. Please try again.");
    }

    public async Task<Result> RequestEmailConfirmation()
    {
        // validate user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _userSession.UserId);
        if (user is null)
            return new ErrorResult("Invalid request: User does not exist");

        string token = CodeGenerator.GenerateCode(100);

        bool saved = await SaveNewCode(user.Id, token, CodePurposes.ConfirmEmail);

        if (!saved)
            return new ErrorResult("Unable to send confirmation email at the moment. Please try again.");

        await _emailService.SendConfirmEmail(user.Email, token);

        return new SuccessResult("Confirmation email sent successfully.");
    }

    public async Task<Result> AuthenticateUser(LoginModel model)
    {
        model.Email = model.Email.ToLower().Trim();
        User user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email);

        if (user == null)
            return new ErrorResult("Login Failed:", "Invalid credentials.");

        if (!user.IsActive)
            return new ErrorResult("Login Failed:", "Account suspended, kindly contact the admin.");

        if (!user.HashedPassword.VerifyPassword(model.Password))
            return new ErrorResult("Login Failed:", "Invalid credentials.");

        return await _tokenGenerator.GenerateJwtToken(user);
    }

    public async Task<Result> RefreshToken(RefreshTokenModel model)
    {
        return await _tokenGenerator.RefreshJwtToken(model.RefreshToken);
    }

    public async Task<Result> Logout(string userReference)
    {
        await _tokenGenerator.InvalidateToken(userReference);
        return new SuccessResult();
    }

    public async Task<Result> UserProfile()
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _userSession.UserId);

        if (user == null)
            return new ErrorResult(StatusCodes.Status404NotFound, "User not found.");

        var userView = user.Adapt<UserProfileView>();

        return new SuccessResult(userView);
    }

    public async Task<Result> RequestForPasswordReset(string email)
    {
        string responseMessage = "If this email is associated with an account, you will receive a password reset email shortly.";

        // validate user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
        if (user is null)
            return new SuccessResult(responseMessage);

        string token = CodeGenerator.GenerateCode(100);

        bool saved = await SaveNewCode(user.Id, token, CodePurposes.ResetPassword);

        if (!saved)
            return new ErrorResult("Unable to send password reset email at the moment. Please try again.");

        var emailRes = await _emailService.SendPasswordResetEmail(user.Email, token);
        if (!emailRes.Success)
            return emailRes;

        return new SuccessResult(responseMessage);
    }

    public async Task<Result> ResetPassword(ResetPasswordModel model)
    {
        // validate user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim());
        if (user is null)
            return new ErrorResult("Invalid request: User does not exist");

        // validate token
        var today = DateTime.UtcNow;
        var code = await _context.Codes
            .FirstOrDefaultAsync(c => c.OwnerId == user.Id
                && c.Purpose == CodePurposes.ResetPassword
                && c.Token == model.Token
                && c.Used == false);

        if (code == null)
            return new ErrorResult("Invalid request, kindly request a new password reset email.");

        if (code.ExpiryDate < today)
            return new ErrorResult("Password reset token has expired. Kindly request a new one.");

        // update user and token
        user.HashedPassword = model.NewPassword.HashPassword();
        code.Used = true;

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Unable to reset password at the moment. Please try again.");

        // send password reset notification Email
        await _emailService.SendEmail(model.Email, "Your Password Was Just Reset - ME Academy", EmailTemplates.PasswordResetNotification);

        return new SuccessResult("Password reset successful.");
    }

    public async Task<Result> InviteUser(UserInvitationModel model)
    {
        // validate user
        bool userExist = await _context.Users
            .AnyAsync(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim());
        if (userExist)
            return new ErrorResult("User already exist in the system");

        // validate invitation
        bool invitationExist = await _context.InvitedUsers
            .AnyAsync(i => i.Email.ToLower().Trim() == model.Email.Trim().ToLower());
        if (invitationExist)
            return new ErrorResult("User is already invited, kindly send a reminder instead.");

        string token = CodeGenerator.GenerateCode(150);

        // get and encode the url with token
        string url =
            $"{_baseUrls.AdminClient}/auth/accept-invitation/{model.Email}/{HttpUtility.UrlEncode(token)}";

        // save invitation
        var invitation = model.Adapt<InvitedUser>();
        invitation.Token = token;
        invitation.CreatedById = _userSession.UserId;

        await _context.AddAsync(invitation);

        int saved = await _context.SaveChangesAsync();

        if (saved < 1)
            return new ErrorResult("Unable to send invitation at the moment. Please try again.");

        // send invitation email
        var args = new Dictionary<string, string?> {
            {
                "url", url
            },
            {
                "name", $"{model.FirstName} {model.LastName}"
            },
            {
                "sender_name", _userSession.Name
            }
        };
        var emailRes = await _emailService.SendEmail(model.Email, "Invitation to ME Academy", EmailTemplates.Invitation, args);

        return emailRes.Success
            ? new SuccessResult("Invitation sent successfully.")
            : new ErrorResult("Invitation details saved successfully, but sending email failed. Kindly retry with a reminder;");
    }

    public async Task<Result> AcceptInvitation(AcceptInvitationModel model)
    {
        // validate invitation
        var invitation = await _context.InvitedUsers
            .Include(i => i.CreatedBy)
            .FirstOrDefaultAsync(i => i.Email.ToLower().Trim() == model.Email.ToLower().Trim()
                && i.Token == model.Token);
        if (invitation == null)
            return new ErrorResult("Invalid invitation.");

        // validate user
        bool userExist = await _context.Users
            .AnyAsync(u => invitation.IsAccepted
                || u.Email.ToLower().Trim() == model.Email.ToLower().Trim());
        if (userExist)
            return new ErrorResult("User already exist in the system");

        // create user object
        var user = invitation.Adapt<User>();
        user.EmailConfirmed = true;
        user.HashedPassword = model.Password.HashPassword();

        // set up user roles
        var userRoles = new List<UserRole> { new() { RoleId = (int)Roles.Manager} };
        if (invitation.CanManageCourses)
            userRoles.Add(new() { RoleId = (int)Roles.ManageCourse });
        if (invitation.CanManageUsers)
            userRoles.Add(new() { RoleId = (int)Roles.ManageUser });

        user.UserRoles = userRoles;

        // update invitation
        invitation.IsAccepted = true;
        invitation.DateAccepted = DateTime.UtcNow;

        // save user
        await _context.AddAsync(user);

        int saved = await _context.SaveChangesAsync();
        if (saved < 1)
            return new ErrorResult("Unable to add user at the moment. Please try again");

        // send invitation accepted email
        var args = new Dictionary<string, string?>
        {
            {
                "name", invitation.CreatedBy?.FirstName
            },
            {
               "member", $"{user.FirstName} {user.LastName}"
            },
            {
                "url", $"{_baseUrls.AdminClient}/auth/users"
            }
        };
        await _emailService.SendEmail(user.Email, "Welcome on board", EmailTemplates.InvitationAccepted, args);

        // create user token
        var authData = await _tokenGenerator.GenerateJwtToken(user);

        // return user token
        if (!authData.Success)
            return new SuccessResult(StatusCodes.Status201Created);

        return new SuccessResult(StatusCodes.Status201Created, authData.Content);
    }

    #region Private Method

    private async Task<bool> SaveNewCode(int userId, string token, string purpose)
    {
        // set all existing code for this user and purpose to used
        var existingCodes = await _context.Codes
            .Where(c => c.OwnerId == userId && c.Purpose == purpose && !c.Used)
            .ToListAsync();

        foreach (var code in existingCodes)
        {
            code.Used = true;
        }

        Code newCode = new()
        {
            OwnerId = userId,
            Token = token,
            Purpose = purpose,
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };
        await _context.AddAsync(newCode);

        int saved = await _context.SaveChangesAsync();
        return saved > 0;
    }

    #endregion
}