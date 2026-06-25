using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task <int> SaveAsync(User user);
    Task <User?> GetByEmailAsync (string email);
    Task <User?> GetByIdAsync (int id);
    Task UpdateAsync(User user);
    void Update(User user);
}