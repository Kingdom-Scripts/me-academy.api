using System.Net.Mail;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IEmailService
{
    Task<Result> SendConfirmEmail(string to, string token);
    Task<Result> SendPasswordResetEmail(ForgotPasswordModel model, string token);
    Task<Result> SendEmail(string to, string subject, string template, Dictionary<string, string?>? args = null);
}