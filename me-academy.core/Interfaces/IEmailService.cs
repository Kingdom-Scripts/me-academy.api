using System.Net.Mail;
using me_academy.core.Models.Utilities;

namespace me_academy.core.Interfaces
{
    public interface IEmailService
    {
        public Result SendMessage(string to, string subject, string body, Attachment? attachment = null);
        Task<Result> SendConfirmEmail(string to, string token);
    }
}