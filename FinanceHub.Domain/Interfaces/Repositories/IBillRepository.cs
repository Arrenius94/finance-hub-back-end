using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Output.Bill;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Interfaces.Repositories;
public record BillQueryResult(EBillStatus Status, int Count, decimal Total);
public interface IBillRepository
{
    Task <int> SaveAsync (Bill bill);
    void Added (Bill bill);
    Task<List<BillQueryResult>> GetThreeMetricsAsync(int userId, CancellationToken ct);
    Task<List<DashboardGraphicView>> GetGraphicDataAsync(DashboarGraphicFilter filter, CancellationToken ct);
    Task<List<Bill>> GetByIdsPayment(List<int> billIds, int userId, CancellationToken ct);
    Task<List<Bill>> GetByIdsDeleteAsync(int[] billIds, int userId, CancellationToken ct);
    void RemoveRange(IEnumerable<Bill> bills);
    Task<List<Bill>> GetPendingBillsForNotificationAsync(DateTime today, CancellationToken ct);
}