using FinanceHub.Domain.DTOS.Input;
using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUser user)
    { 
        var result = await _userService.CreateUserAsync(user);
        
       if (result.IsError)
            return result.ToActionResult();
       
       return Ok(new { id = result.Value });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser user)
    {
        var response = await _userService.LoginUserAsync(user);
        return response.ToActionResult();
    }
    
    [Authorize]
    [HttpPatch("wallet/{id}")]
    public async Task<IActionResult> UpdateWallet([FromRoute] int id, [FromBody] IncreaseWallet request)
    {
        var result = await _userService.UpdateWalletAsync(id, request);
    
        if (result.IsError)
            return result.ToActionResult();
    
        return Ok(new { wallet = result.Value });
    }
    
    [Authorize]
    [HttpPatch("changePassword/{id}")]
    public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] ChangePassword request)
    {
        var result = await _userService.UpdatePasswordAsync(id, request);
        if (result.IsError)
            return result.ToActionResult();
        
        return Ok();
    }

    [Authorize]
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var result = await _userService.GetBalanceAsync();
        if(result.IsError)
            return result.ToActionResult();
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("name")]
    public async Task<IActionResult> GetUserName()
    {
        var result = await _userService.GetUserNameAsync();
        if(result.IsError)
            return result.ToActionResult();
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetUserPerfil()
    {
        var result = await _userService.GetUserPerfilAsync();
        if(result.IsError)
            return result.ToActionResult();
        
        return Ok(result.Value);
    }
}