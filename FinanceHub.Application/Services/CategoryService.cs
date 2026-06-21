using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;

namespace FinanceHub.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<ErrorOr<int>> CreateCategoryAsync(CreateCategory request, int userId)
    {
        var name = request.Name;
        if (string.IsNullOrWhiteSpace(name))
            return AppErrors.Category.NameError;

        var categoryType = request.CategoryType;
        if(categoryType == null)
            return AppErrors.Category.TypeCategoryError;
        
        if (!Enum.IsDefined(typeof(ECategoryType), categoryType.Value))
            return AppErrors.Category.TypeCategoryError;
        
        var category = new Category(
            request.Name, 
            request.CategoryType.Value, 
            userId
        );
        
        await _categoryRepository.SaveAsync(category);
        return category.Id;
    }

    public async Task<ErrorOr<Success>> UpdateCategoryAsync(int categoryId, int userId, UpdateCategory request)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            return AppErrors.Category.NotFound;
        
        if (category.UserId != userId)
            return AppErrors.Category.Unauthorized;
        
        category.Update(request.Name, request.CategoryType);
        await _categoryRepository.UpdateAsync(category);
        return Result.Success;
    }
}