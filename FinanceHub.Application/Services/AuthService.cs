using System.Runtime.InteropServices.JavaScript;
using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.DTOS.Output.User;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Security;
using FinanceHub.Domain.Interfaces.Services;

namespace FinanceHub.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPassWordHasher _passWordHasher;
    private readonly ITokenJwt _tokenJwt;
    private readonly IEmailService _emailService;

    public AuthService(IUserRepository userRepository, IPassWordHasher passWordHasher,  ITokenJwt tokenJwt, IEmailService emailService)
    {
        _userRepository = userRepository;
        _passWordHasher = passWordHasher;
        _tokenJwt = tokenJwt;
        _emailService = emailService;
    }
        
    public async Task<ErrorOr<LoginUserResponse>> LoginUserAsync(LoginUser request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return AppErrors.Authentication.InvalidCredentials;

        var isValidPassword = _passWordHasher.VerifyHashedPassword(request.Password, user.Password);
        if (!isValidPassword)
            return AppErrors.Authentication.InvalidCredentials;

        if (!user.ConfirmEmail)
        {
            var code = Random.Shared.Next(1000, 9999).ToString();

            user.VerifcationCode = code;
            user.CodeExpiration = DateTime.UtcNow.AddMinutes(10);
            
            await _userRepository.UpdateAsync(user);
            
            var html = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2>Olá, {user.Name}!</h2>
                    <p>Seu código de acesso ao FinanceHub é:</p>
                    <h1 style='color: #4F46E5; letter-spacing: 4px;'>{code}</h1>
                    <p>Este código expira em 10 minutos.</p>
                </div>";
            
            await _emailService.SendEmailAsync(user.Email, "Código de Confirmação - FinanceHub",  html);
            
            return AppErrors.Authentication.RequiresEmailVerification;
        }

        var token = _tokenJwt.GenerateJwt(user.Email, user.Id);
        
        return new LoginUserResponse{Username = user.Email, Token = token};
    }

    public async Task<ErrorOr<LoginUserResponse>> VerifyFirstLoginAsync(VerifyCodeRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if(user is null)
            return AppErrors.Authentication.InvalidCredentials;

        if(string.IsNullOrWhiteSpace(request.Code) || request.Code != user.VerifcationCode)
            return AppErrors.Authentication.InvalidCode;

        if (user.CodeExpiration is null || user.CodeExpiration < DateTime.UtcNow)
            return AppErrors.Authentication.CodeExpired;

        user.ConfirmUserEmail();  
        
        await _userRepository.UpdateAsync(user);
        
        var token = _tokenJwt.GenerateJwt(user.Email, user.Id);
        
        return new LoginUserResponse{Username = user.Email, Token = token};
    }
}