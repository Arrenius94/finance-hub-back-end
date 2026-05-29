namespace FinanceHub.Domain.Interfaces.Security;

public interface IPassWordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashPassword);
}