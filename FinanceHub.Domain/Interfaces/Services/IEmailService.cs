namespace FinanceHub.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string bodyHtml, CancellationToken ct);
    Task SendVerificationCodeEmailAsync(string email, string userName, string code, CancellationToken ct = default);
    Task SendNewPasswordEmailAsync(string email, string userName, string newPassword, CancellationToken ct = default);
}