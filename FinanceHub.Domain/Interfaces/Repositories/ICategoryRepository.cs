using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Input.Bill;
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
    Task<bool> HasPendingBillsAsync(int categoryId);
    Task DeleteAsync(Category category);
    Task<CategoryDetailView?> GetViewByIdAsync(int id, int userId, BillFilter filter, CancellationToken ct);
}