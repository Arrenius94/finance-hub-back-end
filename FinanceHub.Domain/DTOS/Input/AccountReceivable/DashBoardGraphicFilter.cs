using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.AccountReceivable;

public sealed record DashBoardGraphicFilter(
    int UserId,
    int? Month = null,
    int? Year = null,
    List<EReceivableStatus>? Status = null,
    List<int>? CategoryIds = null
);
