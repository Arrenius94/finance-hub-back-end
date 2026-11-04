using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.DTOS.Output.AccountReceivable;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Repositories;

public class AccountReceivableRepository : IAccountReceivableRepository
{
    private readonly FinanceHubDbContext _dbContext;
    public AccountReceivableRepository(FinanceHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveAsync(AccountReceivable accountReceivable, CancellationToken ct)
    {
        _dbContext.AccountReceivables.Add(accountReceivable);
        await _dbContext.SaveChangesAsync(ct);

        return accountReceivable.Id;
    }

    public async Task<AccountReceivable?> GetByIdAsync(int userId, int id, CancellationToken ct)
    {
        var query = await _dbContext.AccountReceivables
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);

        return query;
    }

    public async Task<List<AccountReceivableView>> GetAllAsync(int userId, AccountReceivableFilter filter, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-3));

        var query = _dbContext.AccountReceivables
            .AsNoTracking()
            .Where(a => a.UserId == userId);

        var description = filter.Description?.Trim();

        if (!string.IsNullOrWhiteSpace(description) && description.Length >= 3)
        {
            query = query.Where(a => EF.Functions.ILike(a.Description, $"%{description}%"));
        }

        if (filter.ReceiveDate.HasValue)
        {
            query = query.Where(a => a.ReceiveDate == filter.ReceiveDate.Value);
        }

        if (filter.ReceivableStatus.HasValue)
        {
            query = filter.ReceivableStatus.Value switch
            {
                EReceivableStatus.Received => query.Where(a => a.Status == EReceivableStatus.Received),
                EReceivableStatus.Pending => query.Where(a =>
                    a.Status != EReceivableStatus.Received && a.ReceiveDate >= today),
                EReceivableStatus.Overdue => query.Where(a =>
                    a.Status != EReceivableStatus.Received && a.ReceiveDate < today),
                _ => query
            };
        }

        var result = await query
            .Select(a => new AccountReceivableView
            {
                Description = a.Description,
                Value = a.Value,
                CreatedDate = DateOnly.FromDateTime(a.CreatedAt),
                ReceivedDate = a.ReceiveDate,
                Status = a.Status == EReceivableStatus.Received
                    ? EReceivableStatus.Received
                    : a.ReceiveDate < today
                        ? EReceivableStatus.Overdue
                        : EReceivableStatus.Pending
            }).ToListAsync(ct);

        return result;
    }

    public async Task UpdateAsync(AccountReceivable accountReceivable)
    {
        _dbContext.AccountReceivables.Update(accountReceivable);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<AccountReceivable>> GetByIdsAccountReceived(List<int> receivedIds, int userId, CancellationToken ct)
    {
        var result = await _dbContext.AccountReceivables
            .Include(a => a.User)
            .Where(a => receivedIds.Contains(a.Id) && a.UserId == userId && a.Status != EReceivableStatus.Received)
            .ToListAsync(ct);

        return result;
    }

    public async Task<List<AccountReiceivableQueryResult>> GetThreeMetricsAsync(int userId, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-3));

        var query = _dbContext.AccountReceivables
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .GroupBy(a => a.Status == EReceivableStatus.Received
                ? EReceivableStatus.Received
                : a.ReceiveDate < today
                    ? EReceivableStatus.Overdue
                    : EReceivableStatus.Pending)
            .Select(g => new AccountReiceivableQueryResult(
                g.Key,
                g.Count(),
                g.Sum(x => x.Value)
            ));

        return await query.ToListAsync(ct);
    }

    public async Task<List<DashBoardGraphicView>> GetGraphicDataAsync(DashBoardGraphicFilter filter, CancellationToken ct)
    {
        var query = _dbContext.AccountReceivables.AsNoTracking()
            .Where(a => a.UserId == filter.UserId);

        if (filter.Status is { Count: > 0 })
        {
            query = query.Where(a => filter.Status.Contains(a.Status));
        }

        if (filter.Month.HasValue)
        {
            query = query.Where(a => a.ReceiveDate.Month == filter.Month);
        }

        if (filter.Year.HasValue)
        {
            query = query.Where(a => a.ReceiveDate.Year == filter.Year);
        }

        var result = await query
            .GroupBy(a => a.Status)
            .Select(g => new DashBoardGraphicView(
                g.Key,
      g.Sum(x => x.Value)
            ))
            .ToListAsync(ct);

        return result;
    }

    public async Task<List<AccountReceivable>> GetAccountReceivablesByIdsAsync(List<int> ids, int userId, CancellationToken ct)
    {
        var query = await _dbContext.AccountReceivables
            .Where(a => ids.Contains(a.Id) && a.UserId == userId)
            .ToListAsync(ct);

        return query;
    }

    public async Task RemoveRangeAsync(List<int> accountReceivableIds, int userId, CancellationToken ct)
    {
        await _dbContext.AccountReceivables
            .Where(a => accountReceivableIds.Contains(a.Id) && a.UserId == userId)
            .ExecuteDeleteAsync(ct);
    }
}