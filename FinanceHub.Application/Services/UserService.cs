using ErrorOr;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;
using FinanceHub.Domain.Entities;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.Interfaces.Security;

namespace FinanceHub.Application.Services;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPassWordHasher _passWordHasher;
    private readonly ITokenJwt _tokenJwt;
    
    public UserService(IUserRepository userRepository, IPassWordHasher passWordHasher, ITokenJwt tokenJwt)
    {
        _userRepository = userRepository;
        _passWordHasher = passWordHasher;
        _tokenJwt = tokenJwt;
    }
    
    public async Task<ErrorOr<int>> CreateUserAsync(CreateUser request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if(existingEmail != null)
            return AppErrors.User.EmailAlreadyInUse;
        
        var passwordHash = _passWordHasher.HashPassword(request.Password);
        
        var user = new User(
            request.Name,
            request.SecondName,
            request.DateBirth,
            request.Email,
            passwordHash,
            request.Wallet
        );
        await _userRepository.SaveAsync(user);
        return user.Id;
    }

    public async Task<ErrorOr<LoginUserResponse>> LoginUserAsync(LoginUser request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return AppErrors.Authentication.InvalidCredentials;

        var isPasswordValid = _passWordHasher.VerifyHashedPassword(request.Password, user.Password);
        if(!isPasswordValid)
            return AppErrors.Authentication.InvalidCredentials;

        var token = _tokenJwt.GenerateJwt(request.Email);
        return new LoginUserResponse{Username = user.Name, Token = token};
    }

    public async Task<ErrorOr<decimal>> UpdateWalletAsync(int userId, decimal amount)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return AppErrors.User.NotFound;
        
        user.UpdateWallet(amount);
        await _userRepository.UpdateAsync(user);
        
        return user.Wallet ?? 0;
    }
}