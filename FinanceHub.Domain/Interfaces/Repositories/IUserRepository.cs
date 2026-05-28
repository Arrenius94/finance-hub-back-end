using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task <int> SaveAsync(User user);
    Task <User?> LoginAsync (string email, string passwordHash);
}