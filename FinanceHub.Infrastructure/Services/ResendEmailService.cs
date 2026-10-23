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
    public Task SendEmailAsync(string email, string subject, string bodyHtml, CancellationToken ct)
    {
        var message = new EmailMessage()
        {
            From = "FinanceHub <onboarding@resend.dev>",
            To = {email},
            Subject = subject,
            HtmlBody = bodyHtml
        };
        
        return _resend.EmailSendAsync(message, ct);
    }

    public Task SendVerificationCodeEmailAsync(string email, string userName, string code, CancellationToken ct)
    {
        var html = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Olá, {userName}!</h2>
                <p>Seu código de acesso ao FinanceHub é:</p>
                <h1 style='color: #4F46E5; letter-spacing: 4px;'>{code}</h1>
                <p>Este código expira em 10 minutos.</p>
            </div>";

        return SendEmailAsync(email, "Seu código de acesso - FinanceHub", html, ct);
    }

    public Task SendNewPasswordEmailAsync(string email, string userName, string newPassword, CancellationToken ct = default)
    {
        var html = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Olá, {userName}!</h2>
                <p>Sua nova senha de acesso ao FinanceHub é:</p>
                <div style='background-color: #F3F4F6; padding: 10px 20px; border-radius: 6px; display: inline-block;'>
                    <h1 style='color: #4F46E5; letter-spacing: 2px; margin: 0;'>{newPassword}</h1>
                </div>
                <p>Recomendamos alterar esta senha após o seu próximo login.</p>
            </div>";

        return SendEmailAsync(email, "Sua nova senha - FinanceHub", html, ct);
    }
}