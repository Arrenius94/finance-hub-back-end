using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Output.Bill;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public void Added (Bill bill)
    { 
        _dbContext.Bills.Add(bill);
    }
    public async Task<List<BillQueryResult>> GetThreeMetricsAsync(int userId, CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;

        var query = _dbContext.Bills
            .AsNoTracking()
            .Where(x => x.Category.UserId == userId)
            .GroupBy(b => b.DatePayment != null
                ? EBillStatus.Paid
                : b.DateDue < today
                    ? EBillStatus.Overdue
                    : EBillStatus.Pending)
            .Select(g => new BillQueryResult(
                g.Key,
                g.Count(),
                g.Sum(x => x.Value)
            ));
        var result = await query.ToListAsync(ct);

        return result;
    }
    public async Task<List<DashboardGraphicView>> GetGraphicDataAsync(DashboarGraphicFilter filter, CancellationToken ct)
    {
        var query = _dbContext.Bills.AsNoTracking()
            .Where(b => b.Category.UserId == filter.UserId);

        if(filter.Month.HasValue)
        {
            query = query.Where(b => b.DateDue.Month == filter.Month);
        }
        
        if(filter.Year.HasValue)
        {
            query = query.Where(b => b.DateDue.Year == filter.Year);
        }
        
        if(filter.CategoryTypes != null && filter.CategoryTypes.Any())
        {
            query = query.Where(b => filter.CategoryTypes.Contains(b.Category.CategoryType));   
        }
        
        var result = await query 
            .GroupBy(b => b.Category.Name)
            .Select(g => new DashboardGraphicView
            {
                CategoryName = g.Key,
                TotalValue = g.Sum(b => b.Value)
            })
            .ToListAsync(ct);
        
        return result;
    }

    public async Task<List<Bill>> GetByIdsPayment(List<int> billIds, int userId, CancellationToken  ct)
    {
        var result = await _dbContext.Bills
            .Include(b => b.Category)
            .Where(b => billIds.Contains(b.Id) && b.Category.UserId == userId && b.BillStatus != EBillStatus.Paid)
            .ToListAsync(ct);
        
        return result;
    }

    public async Task<List<Bill>> GetByIdsDeleteAsync(int[] billIds, int userId, CancellationToken ct)
    {
        var result = await _dbContext.Bills
            .Where(b => billIds.Contains(b.Id) && b.Category.UserId == userId)
            .ToListAsync(ct);
        return result;
    }

    public void RemoveRange(IEnumerable<Bill> bills)
    {
        _dbContext.Bills.RemoveRange(bills);
    }

    public async Task<List<Bill>> GetPendingBillsForNotificationAsync(DateTime today, CancellationToken ct)
    {
        var dateToday = today.Date;
        var inTreeDays = today.Date.AddDays(3);
        var inSevenDays = today.Date.AddDays(7);

        var query = await _dbContext.Bills
            .Include(b => b.Category)
            .ThenInclude(c => c.User)
            .Where(b => b.BillStatus == EBillStatus.Pending && (
                b.DateDue.Date == dateToday ||
                b.DateDue.Date == inTreeDays ||
                b.DateDue.Date == inSevenDays
            ))
            .ToListAsync(ct);
;        
        return query;
    }
}