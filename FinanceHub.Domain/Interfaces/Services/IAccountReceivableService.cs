using ErrorOr;
using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.DTOS.Output.AccountReceivable;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IAccountReceivableService
{
    Task<ErrorOr<int>> CreateAccountReceivableServiceAsync(CreateAccountReceivableRequest request, CancellationToken ct);
    Task<ErrorOr<AccountReceivableView>> GetAccountReceivableViewAsync(int id, CancellationToken ct);
    Task<ErrorOr<List<AccountReceivableView>>> GetAllReceivableViewAsync(AccountReceivableFilter request, CancellationToken ct);
    Task<ErrorOr<Success>> UpdateAccountReceivableAsync(UpdateAccountReceivable request, int id, CancellationToken ct);
    Task<ErrorOr<Success>> ReceivedListAsync(AccountReceivedListRequest receivedListRequest, CancellationToken ct);
    Task<ErrorOr<DashBoardMetricsView>> GetThreeMetricsAsync(CancellationToken ct);
    Task<ErrorOr<List<DashBoardGraphicView>>> GetGraphicDataAsync(DashBoardGraphicFilter filter, CancellationToken ct);
}