namespace FinanceHub.Domain.Interfaces.Services;

public interface ITokenJwt
{
    string GenerateJwt (string email, int userId);
}