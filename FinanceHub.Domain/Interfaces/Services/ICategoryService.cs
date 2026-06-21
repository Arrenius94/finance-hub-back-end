using ErrorOr;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Services;

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateCategoryAsync(CreateCategory request, int userId);
    Task<ErrorOr<Success>> UpdateCategoryAsync(int categoryId, int userId, UpdateCategory request);
}