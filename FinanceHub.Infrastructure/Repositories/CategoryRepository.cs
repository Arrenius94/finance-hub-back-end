using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Input.Category;
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

    public async Task<PagedResult<CategoryView>> GetAllAsync(CategoryFilter filter)
    {
        var day = DateTime.UtcNow.AddHours(-3).Date;

        var query = _dbContext.Categories.AsNoTracking();

        if(filter.RestrictUserId is not null)
        {
            query = query.Where(c => c.UserId == filter.RestrictUserId);
        }

        query = query.Where(c => c.Bills.Any(b =>
                (filter.BillStatus == null ||
                (filter.BillStatus == EBillStatus.Paid && b.DatePayment != null) ||
                (filter.BillStatus == EBillStatus.Pending && b.DatePayment == null && b.DateDue >= day) ||
                (filter.BillStatus == EBillStatus.Overdue && b.DatePayment == null && b.DateDue < day))
                &&
                (filter.StartDate == null || b.DateDue >= filter.StartDate)
                &&
                (filter.EndDate == null || b.DateDue <= filter.EndDate)
            ) || !c.Bills.Any());

        var totalCount = await query.CountAsync();

        var projectedQuery = query
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .Select(c => new CategoryView
            {
                Id = c.Id,
                Name = c.Name,
                CategoryType = c.CategoryType,
                Bills = c.Bills
                    .Where(b =>
                        (filter.BillStatus == null ||
                         (filter.BillStatus == EBillStatus.Paid && b.DatePayment != null) ||
                         (filter.BillStatus == EBillStatus.Pending && b.DatePayment == null && b.DateDue >= day) ||
                         (filter.BillStatus == EBillStatus.Overdue && b.DatePayment == null && b.DateDue < day))
                        &&
                        (filter.StartDate == null || b.DateDue >= filter.StartDate)
                        &&
                        (filter.EndDate == null || b.DateDue <= filter.EndDate))
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
                                : EBillStatus.Pending
                    })
                    .ToList()
            });

        return await projectedQuery.ToPagedListAsync(filter.PageIndex, filter.PageSize, totalCount);
    }

    public async Task<bool> HasPendingBillsAsync(int categoryId)
    {
        var result = await _dbContext.Bills
            .AnyAsync(b => b.CategoryId == categoryId && 
                           (b.DatePayment == null || 
                            b.BillStatus == EBillStatus.Pending || 
                            b.BillStatus == EBillStatus.Overdue));
        return result;
    }

    public async Task DeleteAsync(Category category)
    {
       _dbContext.Categories.Remove(category);
       await _dbContext.SaveChangesAsync();
    }
};