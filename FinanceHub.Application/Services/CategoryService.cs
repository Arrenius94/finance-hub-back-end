using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Category;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Infrastructure.Security;

namespace FinanceHub.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUser _currentUser;
    
    public CategoryService(ICategoryRepository categoryRepository, ICurrentUser currentUser)
    {
        _categoryRepository = categoryRepository;
        _currentUser = currentUser;
    } 
    
    public async Task<ErrorOr<int>> CreateCategoryAsync(CreateCategory request)
    {
        var name = request.Name;
        if (string.IsNullOrWhiteSpace(name))
            return AppErrors.Category.NameError;

        var categoryType = request.CategoryType;
        if(categoryType == null)
            return AppErrors.Category.TypeCategoryError;
        
        if (!Enum.IsDefined(typeof(ECategoryType), categoryType.Value))
            return AppErrors.Category.TypeCategoryError;
        
        var userId = _currentUser.UserId;
        
        var category = new Category(
            name, 
            categoryType.Value, 
            userId
        );
        
        await _categoryRepository.SaveAsync(category);
        return category.Id;
    }

    public async Task<ErrorOr<Success>> UpdateCategoryAsync(int categoryId, UpdateCategory request)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            return AppErrors.Category.NotFound;
        
        var userId = _currentUser.UserId;
        if (category.UserId != userId)
            return AppErrors.Category.Unauthorized;
        
        if (string.IsNullOrWhiteSpace(request.Name))
            return AppErrors.Category.NameError;

        category.Update(request.Name, request.CategoryType);
        await _categoryRepository.UpdateAsync(category);
        return Result.Success;
    }

    public async Task<ErrorOr<List<CategoryView>>> GetAllAsync(CategoryFilter filter)
    {
        var userId = _currentUser.UserId;
        
        if (userId <= 0)
            return AppErrors.User.NotFound;
        
        filter.RestrictUserId = userId;
        
        var category = await _categoryRepository.GetAllAsync(filter);
        
        if (category is null)
            return AppErrors.Category.NotFound;

        return category;
    }

    public async Task<ErrorOr<Success>> DeleteCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
        {
            return AppErrors.Category.NotFound;
        }

        var userId = _currentUser.UserId;
        if (category.UserId != userId)
        {
            return AppErrors.Category.Unauthorized;
        }

        var hasPendingBills = await _categoryRepository.HasPendingBillsAsync(categoryId);
        if (hasPendingBills)
        {
            return AppErrors.Category.HasPendingBills(category.Name);
        }
        await _categoryRepository.DeleteAsync(category);
        return Result.Success;
    }
}