using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Bill;
using FinanceHub.Domain.DTOS.Output.Category;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using FinanceHub.Infrastructure.Extensions;
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

    public async Task<PagedResult<CategoryView>> GetAllAsync(int userId, CategoryFilter filter, CancellationToken ct)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .Select(c => new CategoryView
            {
                Id = c.Id,
                Name = c.Name,
                CategoryType = c.CategoryType
            });

        return await query.ToPagedListAsync(filter.PageIndex, filter.PageSize, ct);
    }

    public async Task DeleteAsync(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CategoryDetailView?> GetViewByIdAsync(int id, int userId, CategoryBillsFilter filter, CancellationToken ct)
    {
        var category = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id == id && c.UserId == userId)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.CategoryType
            })
            .FirstOrDefaultAsync(ct);

        if (category is null)
            return null;

        var day = DateTime.UtcNow.AddHours(-3).Date;
        var billsQuery = ApplyBillFilter(
            _dbContext.Bills.AsNoTracking().Where(b => b.CategoryId == id),
            filter,
            day);

        var bills = await billsQuery
            .OrderBy(b => b.DateDue)
            .ThenBy(b => b.Id)
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
                CategoryId = b.CategoryId,
                InstallmentNumber = b.InstallmentNumber,
                TotalInstallments = b.TotalInstallments
            })
            .ToPagedListAsync(filter.PageIndex, filter.PageSize, ct);

        return new CategoryDetailView
        {
            Id = category.Id,
            Name = category.Name,
            CategoryType = category.CategoryType,
            Bills = bills
        };
    }

    private static IQueryable<Bill> ApplyBillFilter(IQueryable<Bill> bills, CategoryBillsFilter filter, DateTime day)
    {
        if (filter.BillStatus is not null)
        {
            bills = filter.BillStatus switch
            {
                EBillStatus.Paid => bills.Where(b => b.DatePayment != null),
                EBillStatus.Pending => bills.Where(b => b.DatePayment == null && b.DateDue >= day),
                EBillStatus.Overdue => bills.Where(b => b.DatePayment == null && b.DateDue < day),
                _ => bills
            };
        }

        if (filter.StartDate is not null)
            bills = bills.Where(b => b.DateDue >= filter.StartDate);

        if (filter.EndDate is not null)
            bills = bills.Where(b => b.DateDue <= filter.EndDate);

        return bills;
    }

    public async Task<bool> ExistsByNameAsync(int userId, string name, int? ignoreCategoryId = null)
    {
        var normalized = name.ToLower();

        return await _dbContext.Categories
            .AsNoTracking()
            .AnyAsync(c => c.UserId == userId
                        && c.Name.ToLower() == normalized
                        && (ignoreCategoryId == null || c.Id != ignoreCategoryId));
    }

    public async Task MoveBillsAndDeleteAsync(Category category, int targetCategoryId, int userId, CancellationToken ct)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        await _dbContext.Bills
            .Where(b => b.CategoryId == category.Id && b.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.CategoryId, targetCategoryId), ct);

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(ct);

        await transaction.CommitAsync(ct);
    }

    public async Task<bool> HasAnyBillsAsync(int categoryId)
    {
        return await _dbContext.Bills.AnyAsync(b => b.CategoryId == categoryId);
    }
}