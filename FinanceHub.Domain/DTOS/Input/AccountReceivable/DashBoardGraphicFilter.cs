using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.AccountReceivable;

public sealed record DashBoardGraphicFilter(
       int UserId,
       List<EReceivableStatus>? Status,
       int? Month = null,
       int? Year = null
    );