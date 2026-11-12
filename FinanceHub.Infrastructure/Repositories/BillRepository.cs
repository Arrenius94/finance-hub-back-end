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

    public async Task<int> SaveAsync(Bill bill)
    {
        await _dbContext.Bills.AddAsync(bill);
        await _dbContext.SaveChangesAsync();

        return bill.Id;
    }

    public void Added(Bill bill)
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

        if (filter.Month.HasValue)
        {
            query = query.Where(b => b.DateDue.Month == filter.Month);
        }

        if (filter.Year.HasValue)
        {
            query = query.Where(b => b.DateDue.Year == filter.Year);
        }

        if (filter.CategoryTypes is { Count: > 0 })
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

    public async Task<List<Bill>> GetByIdsPayment(List<int> billIds, int userId, CancellationToken ct)
    {
        var result = await _dbContext.Bills
            .Include(b => b.Category)
            .ThenInclude(c => c.User)
            .Where(b => billIds.Contains(b.Id) && b.Category.UserId == userId && b.BillStatus != EBillStatus.Paid)
            .ToListAsync(ct);

        return result;
    }

    public async Task<List<int>> GetByIdsDeleteAsync(int[] billIds, int userId, CancellationToken ct)
    {
        var result = await _dbContext.Bills
            .Where(b => billIds.Contains(b.Id) && b.Category.UserId == userId)
            .Select(b => b.Id)
            .ToListAsync(ct);
        return result;
    }

    public async Task RemoveRangeAsync(List<int> billsIds, int userId, CancellationToken ct)
    {
        await _dbContext.Bills
            .Where(b => billsIds.Contains(b.Id) && b.Category.UserId == userId)
            .ExecuteDeleteAsync(ct);
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

    public async Task<BillView?> GetByIdAsync(int userId, int id, CancellationToken ct)
    {
        var day = DateTime.UtcNow.AddHours(-3).Date;
        var bill = await _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.Id == id && b.Category.UserId == userId)
            .Select(b => new BillView
            {
                Id = b.Id,
                Description = b.Description,
                Value = b.Value,
                DateDue = b.DateDue,
                DatePayment = b.DatePayment,
                BillStatus = b.DatePayment != null
                                ? EBillStatus.Paid
                                : b.DateDue < day
                                    ? EBillStatus.Overdue
                                    : EBillStatus.Pending,
                InstallmentNumber = b.InstallmentNumber,
                TotalInstallments = b.TotalInstallments
            })
            .FirstOrDefaultAsync(ct);
        return bill;
    }

    public async Task<BillDetailView?> GetInstallmentsByBillIdAsync(int userId, int id, CancellationToken ct)
    {
        var day = DateTime.UtcNow.AddHours(-3).Date;

        var source = await _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.Id == id && b.Category.UserId == userId)
            .Select(b => new { b.Id, b.InstallmentGroupId })
            .FirstOrDefaultAsync(ct);

        if (source is null)
            return null;

        var query = _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.Category.UserId == userId);

        if (source.InstallmentGroupId.HasValue)
            query = query.Where(b => b.InstallmentGroupId == source.InstallmentGroupId);
        else
            query = query.Where(b => b.Id == id);

        var bills = await query
            .OrderBy(b => b.InstallmentNumber)
            .ThenBy(b => b.DateDue)
            .Select(b => new BillView
            {
                Id = b.Id,
                Description = b.Description,
                Value = b.Value,
                DateDue = b.DateDue,
                DatePayment = b.DatePayment,
                BillStatus = b.DatePayment != null
                    ? EBillStatus.Paid
                    : b.DateDue < day
                        ? EBillStatus.Overdue
                        : EBillStatus.Pending,
                InstallmentNumber = b.InstallmentNumber,
                TotalInstallments = b.TotalInstallments
            })
            .ToListAsync(ct);

        if (bills.Count == 0)
            return null;

        var first = bills[0];

        return new BillDetailView
        {
            Description = first.Description,
            TotalValue = bills.Sum(b => b.Value),
            TotalInstallments = first.TotalInstallments,
            PaidCount = bills.Count(b => b.BillStatus == EBillStatus.Paid),
            Bills = bills
        };
    }

    public async Task<Bill?> GetBillIdAsync(int id, int userId, CancellationToken ct)
    {
        return await _dbContext.Bills
            .Where(b => b.Id == id && b.Category.UserId == userId)
            .FirstOrDefaultAsync(ct);
    }
}