using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FinanceHubDbContext _dbContext;
    
    public CategoryRepository(FinanceHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> SaveAsync(Category category)
    {
       await _dbContext.Categories.AddAsync(category);
       await _dbContext.SaveChangesAsync();
       
       return category.Id;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var query = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return query;
    }

    public async Task UpdateAsync(Category category)
    {
         _dbContext.Categories.Update(category);
         await _dbContext.SaveChangesAsync();
    }
}