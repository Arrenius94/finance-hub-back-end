using ErrorOr;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ErrorOr<int>> CreateUserAsync(CreateUser request);
    Task<ErrorOr<LoginUserResponse>> LoginUserAsync(LoginUser request);
    Task<ErrorOr<decimal>> UpdateWalletAsync(int userId, IncreaseWallet amount);
    Task<ErrorOr<string>> UpdatePasswordAsync(int userId, ChangePassword newPassword);
    Task<ErrorOr<BalanceUserView>> GetBalanceAsync();
    Task<ErrorOr<UserNameView>> GetUserNameAsync();
    Task<ErrorOr<UserPerfilView>> GetUserPerfilAsync();
}