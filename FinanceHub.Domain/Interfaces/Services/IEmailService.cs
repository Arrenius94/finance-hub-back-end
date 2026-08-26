namespace FinanceHub.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string bodyHtml);
}