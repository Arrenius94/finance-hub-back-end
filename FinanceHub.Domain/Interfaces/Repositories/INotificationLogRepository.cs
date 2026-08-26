using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface INotificationLogRepository
{
    Task<List<int>> GetNotificationBillsIdsTodayAsync
    (
        List<int> billsIds, 
        ENotificationType notificationType,
        DateTime today, 
        CancellationToken cancellationToken
    );
    Task AddLogAsync(int billId, ENotificationType notificationType,  CancellationToken cancellationToken);
}