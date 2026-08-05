using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Category;
using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task <int> SaveAsync(Category category);
    Task <Category?> GetByIdAsync(int id);
    Task UpdateAsync(Category category);
    Task<List<CategoryView>> GetAllAsync(CategoryFilter filter);
    Task<bool> HasPendingBillsAsync(int categoryId);
    Task DeleteAsync(Category category);
}