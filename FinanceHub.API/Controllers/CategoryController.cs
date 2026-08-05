using System.Security.Claims;
using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICurrentUser _currentUser;
    
    public CategoryController(ICategoryService categoryService,  ICurrentUser currentUser)
    {
        _categoryService = categoryService;
        _currentUser = currentUser;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategory request)
    {
        var result = await _categoryService.CreateCategoryAsync(request);
        if (result.IsError)
            return result.ToActionResult();
        
        return Ok(new { id = result.Value });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategory request)
    {
        var result = await _categoryService.UpdateCategoryAsync(id, request);

        if (result.IsError)
            return result.ToActionResult();

        return NoContent();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] CategoryFilter filter)
    {
        var result = await _categoryService.GetAllAsync(filter);
        
        if (result.IsError)
            return result.ToActionResult();

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);

        if (result.IsError)
            return result.ToActionResult();

        return NoContent();
    }
}