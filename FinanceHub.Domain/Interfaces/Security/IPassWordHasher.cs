namespace FinanceHub.Domain.Interfaces.Security;

public interface IPassWordHasher
{
    string HashPassword(string password);
}