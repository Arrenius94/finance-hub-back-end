using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    
    public  AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUser request)
    {
        var result = await _authService.LoginUserAsync(request);
        if (result.IsError) 
            return result.ToActionResult();
        
        return Ok(result.Value);
    }

    [HttpPost("verify-login")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyLogin([FromBody] VerifyCodeRequest request)
    {
        var result = await _authService.VerifyFirstLoginAsync(request);
        if (result.IsError) 
            return result.ToActionResult();
        
        return Ok(result.Value);
    }
}