using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUser user)
    { 
        var id = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(CreateUser), new { id }, user);
    }
}