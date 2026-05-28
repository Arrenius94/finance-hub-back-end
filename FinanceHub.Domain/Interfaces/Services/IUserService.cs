using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUser request);
    Task<LoginUserResponse?> LoginUserAsync(LoginUser request);
}