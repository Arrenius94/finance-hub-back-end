using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Repositories;

public class NotificationLogRepository : INotificationLogRepository
{
    private readonly FinanceHubDbContext _dbContext;
    
    public NotificationLogRepository (FinanceHubDbContext dbContext)
    {
        _dbContext =  dbContext;
    }
    
    public async Task<List<int>> GetNotificationBillsIdsTodayAsync
    (
        List<int> billsIds,
        ENotificationType notificationType,
        DateTime today,
        CancellationToken cancellationToken)
    {
        var dateToday = today.Date;

        var query = await _dbContext.NotificationLogs
            .AsNoTracking()
            .Where(x => billsIds.Contains(x.BillId)
                        && x.NotificationType == notificationType
                        && x.SentAt.Date == dateToday)
            .Select(x => x.BillId)
            .ToListAsync(cancellationToken);

        return query;
    }
    

    public async Task AddLogAsync(int billId, ENotificationType notificationType, CancellationToken cancellationToken)
    {
        var log = new NotificationLog
        {
            BillId = billId,
            NotificationType = notificationType,
            SentAt = DateTime.UtcNow
        };
        
        await _dbContext.NotificationLogs.AddAsync(log, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}