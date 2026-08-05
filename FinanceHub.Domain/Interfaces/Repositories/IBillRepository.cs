using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.Entities;

namespace FinanceHub.Domain.Interfaces.Repositories;

public interface IBillRepository
{
    Task <int> SaveAsync (Bill bill);
    void Added (Bill bill);
    Task<List<BillQueryResult>> GetThreeMetricsAsync(int userId);
    Task<List<ChartQueryResult>> GetGraphicDataAsync(DashboardChartFilter filter);
    Task<List<Bill>> GetByIdsPayment(List<int> billIds, int userId);
}