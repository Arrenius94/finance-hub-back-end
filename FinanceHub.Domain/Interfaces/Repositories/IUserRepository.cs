using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task <int> SaveAsync(User user);
    Task <User?> GetByEmailAsync (string email);
}