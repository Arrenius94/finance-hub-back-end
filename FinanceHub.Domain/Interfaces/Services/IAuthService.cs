using ErrorOr;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<ErrorOr<LoginUserResponse>> LoginUserAsync(LoginUser request);
    Task<ErrorOr<LoginUserResponse>> VerifyFirstLoginAsync(VerifyCodeRequest request);
}