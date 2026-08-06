using ErrorOr;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Output.Bill;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IBillService
{
    Task<ErrorOr<int>> CreateBillAsync (CreateBillRequest request);
    Task<ErrorOr<DashboardMetricsView>> GetDashboardMetricsAsync();
    Task<ErrorOr<List<DashboardChartView>>> GetDashboardChartAsync(DashboardChartFilter filter);
    Task<ErrorOr<Success>> PayBillListAsync(PayBillsListRequest request);
    Task<ErrorOr<Success>> DeleteBillAsync(DeleteBillsRequest request);
}