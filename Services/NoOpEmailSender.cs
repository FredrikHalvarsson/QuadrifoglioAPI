using Microsoft.AspNetCore.Identity.UI.Services;

namespace QuadrifoglioAPI.Services
{
    public class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No-op email sender does nothing
            return Task.CompletedTask;
        }
    }
}
