using System.Security.Claims;
using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategory request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Usuário não identificado.");
        }

        int userId = int.Parse(userIdClaim);
        
        var result = await _categoryService.CreateCategoryAsync(request, userId);
        if (result.IsError)
            return result.ToActionResult();
       
        return Ok(new { id = result.Value });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategory request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Usuário não identificado.");
        }

        int userId = int.Parse(userIdClaim);
        var result = await _categoryService.UpdateCategoryAsync(id, userId, request);

        if (result.IsError)
            return result.ToActionResult();

        return NoContent();
    }
}