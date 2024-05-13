using System.Net.Mail;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces;

public interface IEmailService
{
    Task<Result> SendConfirmEmail(string to, string token);
    Task<Result> SendPasswordResetEmail(string email, string token);
    Task<Result> SendEmail(string to, string template, Dictionary<string, string>? args = null);
}
