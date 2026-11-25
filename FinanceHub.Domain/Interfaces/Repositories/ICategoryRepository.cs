using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Category;
using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task <int> SaveAsync(Category category);
    Task <Category?> GetByIdAsync(int id);
    Task UpdateAsync(Category category);
    Task<PagedResult<CategoryView>> GetAllAsync(int userId, CategoryFilter filter, CancellationToken ct);
    Task<bool> ExistsByNameAsync(int userId, string name, int? ignoreCategoryId = null);
    Task<bool> HasAnyBillsAsync(int categoryId);
    Task DeleteAsync(Category category);
    Task MoveBillsAndDeleteAsync(Category category, int targetCategoryId, int userId, CancellationToken ct);
    Task<CategoryDetailView?> GetViewByIdAsync(int id, int userId, CategoryBillsFilter filter, CancellationToken ct);
}