using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;

namespace FinanceHub.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly FinanceHubDbContext _dbContext;
    public BillRepository(FinanceHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> SaveAsync (Bill bill)
    {
        await _dbContext.Bills.AddAsync(bill);
        await _dbContext.SaveChangesAsync();
        
        return bill.Id;
    }

    public void Commit (Bill bill)
    { 
        _dbContext.Bills.Add(bill);
    }
}