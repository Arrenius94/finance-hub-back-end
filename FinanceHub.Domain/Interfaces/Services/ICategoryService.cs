using ErrorOr;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Category;
using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Services;

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateCategoryAsync(CreateCategory request);
    Task<ErrorOr<Success>> UpdateCategoryAsync(int categoryId, UpdateCategory request);
    Task<ErrorOr<List<CategoryView>>> GetAllAsync(CategoryFilter filter);
    Task<ErrorOr<Success>> DeleteCategoryAsync(int categoryId);
}