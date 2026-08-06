using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task <int> SaveAsync (User user);
    Task <User?> GetByEmailAsync (string email);
    Task <User?> GetByIdAsync (int id);
    Task UpdateAsync (User user);
    Task <decimal?> GetBalanceAsync (int userId);
    Task <string?> GetByNameAsync (int userId);
    void AttachForUpdate (User user);
    Task DeleteAsync (User user);
}