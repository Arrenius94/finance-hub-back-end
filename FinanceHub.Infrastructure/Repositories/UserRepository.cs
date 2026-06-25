using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FinanceHubDbContext _dbContext;
    public UserRepository(FinanceHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var query = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return query;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var query = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return query;
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}