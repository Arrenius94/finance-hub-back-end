using FinanceHub.Domain.Interfaces.Security;

namespace FinanceHub.Infrastructure.Security;

public class PassWordHasher : IPassWordHasher
{
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
}