using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Output.AccountReceivable;

public sealed record DashBoardGraphicView(
        EReceivableStatus Status,
        decimal TotalValue
    );