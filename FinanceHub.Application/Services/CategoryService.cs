using ErrorOr;
using FinanceHub.Application.Common;
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
        var name = request.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return AppErrors.Category.NameRequired;

        if (request.CategoryType is null || !Enum.IsDefined(typeof(ECategoryType), request.CategoryType.Value))
            return AppErrors.Category.NatureRequired;

        var userId = _currentUser.UserId;

        if (await _categoryRepository.ExistsByNameAsync(userId, name))
            return AppErrors.Category.NameInUse;

        var category = new Category(name, request.CategoryType.Value, userId);

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

        var name = request.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return AppErrors.Category.NameRequired;

        if (await _categoryRepository.ExistsByNameAsync(userId, name, categoryId))
            return AppErrors.Category.NameInUse;

        category.Update(name);

        await _categoryRepository.UpdateAsync(category);
        return Result.Success;
    }

    public async Task<ErrorOr<PagedResult<CategoryView>>> GetAllAsync(CategoryFilter filter, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        if (userId <= 0)
            return AppErrors.User.NotFound;

        return await _categoryRepository.GetAllAsync(userId, filter, ct);
    }

    public async Task<ErrorOr<Success>> DeleteCategoryAsync(int categoryId, int? moveToCategoryId = null, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            return AppErrors.Category.NotFound;

        var userId = _currentUser.UserId;
        if (category.UserId != userId)
            return AppErrors.Category.Unauthorized;

        var hasBills = await _categoryRepository.HasAnyBillsAsync(categoryId);

        if (!hasBills)
        {
            await _categoryRepository.DeleteAsync(category);
            return Result.Success;
        }

        if (moveToCategoryId is null)
            return AppErrors.Category.HasBills(category.Name);

        if (moveToCategoryId.Value == categoryId)
            return AppErrors.Category.SameCategory;

        var target = await _categoryRepository.GetByIdAsync(moveToCategoryId.Value);
        if (target is null)
            return AppErrors.Category.NotFound;

        if (target.UserId != userId)
            return AppErrors.Category.Unauthorized;

        if (target.CategoryType != category.CategoryType)
            return AppErrors.Category.WrongNature;

        await _categoryRepository.MoveBillsAndDeleteAsync(category, target.Id, userId, ct);

        return Result.Success;
    }

    public async Task<ErrorOr<CategoryDetailView>> GetByIdAsync(int id, CategoryBillsFilter filter, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var result = await _categoryRepository.GetViewByIdAsync(id, userId, filter, ct);
        if (result is null)
            return AppErrors.Category.NotFound;

        return result;
    }
}