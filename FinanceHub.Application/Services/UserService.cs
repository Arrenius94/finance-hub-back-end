using ErrorOr;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;
using FinanceHub.Domain.Entities;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.Interfaces.Security;
using FinanceHub.Infrastructure.Security;

namespace FinanceHub.Application.Services;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPassWordHasher _passWordHasher;
    private readonly ITokenJwt _tokenJwt;
    private readonly ICurrentUser _currentUser;
    
    public UserService(IUserRepository userRepository, IPassWordHasher passWordHasher, ITokenJwt tokenJwt, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _passWordHasher = passWordHasher;
        _tokenJwt = tokenJwt;
        _currentUser = currentUser;
    }
    
    public async Task<ErrorOr<int>> CreateUserAsync(CreateUser request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if(existingEmail is not null)
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
        if (user is null)
            return AppErrors.Authentication.InvalidCredentials;

        var isPasswordValid = _passWordHasher.VerifyHashedPassword(request.Password, user.Password);
        if(!isPasswordValid)
            return AppErrors.Authentication.InvalidCredentials;

        var token = _tokenJwt.GenerateJwt(request.Email, user.Id);
        return new LoginUserResponse{Username = user.Name, Token = token};
    }

    public async Task<ErrorOr<decimal>> UpdateWalletAsync(int userId, IncreaseWallet amount)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return AppErrors.User.NotFound;
        
        user.IncreaseValue(amount.Amount);
        await _userRepository.UpdateAsync(user);
        
        return user.Wallet ?? 0;
    }

    public async Task<ErrorOr<string>> UpdatePasswordAsync(int userId, ChangePassword changePassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return AppErrors.User.NotFound;
        
        /*if (changePassword.NewPassword != changePassword.NewPasswordConfirmation)
            return AppErrors.User.PasswordsDoNotMatch;*/
        
        var passwordHash = _passWordHasher.HashPassword(changePassword.NewPassword);
        user.ChangePassword(passwordHash);
        await _userRepository.UpdateAsync(user);
            
        return user.Password;
    }

    public async Task<ErrorOr<BalanceUserView>> GetBalanceAsync()
    {
        var userId = _currentUser.UserId;
        
        var balance = await _userRepository.GetBalanceAsync(userId);
        
        if (balance is null)
            return AppErrors.User.NotFound;

        var userBalance = new BalanceUserView
        (
            Balance : balance.Value
        );
        
        return userBalance;
    }

    public async Task<ErrorOr<UserNameView>> GetUserNameAsync()
    {
        var userId = _currentUser.UserId;
        
        var name = await _userRepository.GetByNameAsync(userId);
        
        if (name is null)
            return AppErrors.User.NotFound;
        
        var userName = new UserNameView
        (
           Name: name
        );
        
        return userName;
    }

    public async Task<ErrorOr<UserPerfilView>> GetUserPerfilAsync()
    {
        var userId = _currentUser.UserId;
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return AppErrors.User.NotFound;
        
        var userPerfil = new UserPerfilView
        (
            Name: user.Name,
            SecondName: user.SecondName,
            Email: user.Email,
            BirthDate: DateOnly.FromDateTime(user.DateBirth) 
        );

        return userPerfil;
    }
    
    public async Task<ErrorOr<Success>> DeleteUserAsync()
    {
        var userId = _currentUser.UserId;
        if (userId <= 0)
            return AppErrors.User.NotFound;
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return AppErrors.User.NotFound;

        await _userRepository.DeleteAsync(user);
        return Result.Success;
    }
}