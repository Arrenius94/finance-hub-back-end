
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Security;

namespace FinanceHub.Application.Services.Users;
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
    
    public async Task<int> CreateUserAsync(CreateUser request)
    {
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

    public async Task<LoginUserResponse?> LoginUserAsync(LoginUser request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) return null;

        var isPasswordValid = _passWordHasher.VerifyHashedPassword(request.Password, user.Password);
        if(!isPasswordValid) return null;

        var token = _tokenJwt.GenerateJwt(request.Email);
        return new LoginUserResponse{Username = user.Name, Token = token};
    }
}