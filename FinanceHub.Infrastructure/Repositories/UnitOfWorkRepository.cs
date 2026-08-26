using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;

namespace FinanceHub.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FinanceHubDbContext _dbContext;

    public UnitOfWork(FinanceHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CommitAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}