using ErrorOr;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ErrorOr<int>> CreateUserAsync(CreateUser request);
    Task<ErrorOr<LoginUserResponse>> LoginUserAsync(LoginUser request);
    Task<ErrorOr<decimal>> UpdateWalletAsync(int userId, decimal amount);
}