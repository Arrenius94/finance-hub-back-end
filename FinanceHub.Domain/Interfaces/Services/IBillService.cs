using ErrorOr;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Output.Bill;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IBillService
{
    Task<ErrorOr<int>> CreateBillAsync (CreateBillRequest request, CancellationToken ct);
    Task<ErrorOr<DashboardMetricsView>> GetDashboardMetricsAsync(CancellationToken ct);
    Task<ErrorOr<List<DashboardGraphicView>>> GetDashboardChartAsync(DashboarGraphicFilter filter, CancellationToken ct);
    Task<ErrorOr<Success>> PayBillListAsync(PayBillsListRequest request, CancellationToken ct);
    Task<ErrorOr<Success>> DeleteBillAsync(DeleteBillsRequest request, CancellationToken ct);
}