using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.Entities;
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
        var id = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(CreateUser), new { id }, user);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser user)
    {
        var response = await _userService.LoginUserAsync(user);
        if (response == null) return Unauthorized(new {message = "Credenciais inválidas."});
        return Ok(response);
    }
}