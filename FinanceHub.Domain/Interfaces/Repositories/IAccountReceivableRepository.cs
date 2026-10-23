using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.DTOS.Output.AccountReceivable;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Interfaces.Repositories;

public record AccountReiceivableQueryResult(EReceivableStatus Status, int Count, decimal Total);
public interface IAccountReceivableRepository
{
    Task<int> SaveAsync(AccountReceivable accountReceivable, CancellationToken ct);
    Task<AccountReceivable?> GetByIdAsync(int userId, int id, CancellationToken ct);
    Task<List<AccountReceivableView>> GetAllAsync(int userId, AccountReceivableFilter filter, CancellationToken ct);
    Task UpdateAsync(AccountReceivable accountReceivable);
    Task<List<AccountReceivable>> GetByIdsAccountReceived(List<int> receivedIds, int userId, CancellationToken ct);
    Task<List<AccountReiceivableQueryResult>> GetThreeMetricsAsync(int userId, CancellationToken ct);
    Task<List<DashBoardGraphicView>> GetGraphicDataAsync(DashBoardGraphicFilter filter, CancellationToken ct);
}