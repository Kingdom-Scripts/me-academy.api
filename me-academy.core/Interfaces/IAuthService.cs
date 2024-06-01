using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IAuthService
{
    Task<Result> Register(RegisterModel model);
    Task<Result> RequestEmailConfirmation();
    Task<Result> ConfirmEmail(ConfirmEmailModel model);
    Task<Result> AuthenticateUser(LoginModel model);
    Task<Result> RefreshToken(RefreshTokenModel model);
    Task<Result> Logout(string userReference);
    Task<Result> RequestForPasswordReset(ForgotPasswordModel model);
    Task<Result> ResetPassword(ResetPasswordModel model);
}