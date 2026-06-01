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
    }}