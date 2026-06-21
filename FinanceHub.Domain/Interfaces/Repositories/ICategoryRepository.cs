using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task <int> SaveAsync (Category category);
    Task <Category?> GetByIdAsync (int id);
    Task UpdateAsync (Category category);
}