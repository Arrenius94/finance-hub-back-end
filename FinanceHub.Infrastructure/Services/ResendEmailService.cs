using FinanceHub.Domain.Interfaces.Services;
using Resend;

namespace FinanceHub.Infrastructure.Services;

public class ResendEmailService : IEmailService
{
    private readonly IResend _resend;

    public ResendEmailService(IResend resend)
    {
        _resend = resend;
    }
    public Task SendEmailAsync(string email, string subject, string bodyHtml)
    {
        var message = new EmailMessage()
        {
            From = "FinanceHub <onboarding@resend.dev>",
            To = {email},
            Subject = subject,
            HtmlBody = bodyHtml
        };
        
        return _resend.EmailSendAsync(message);
    }
}