using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Output.AccountReceivable;

public record AccountReceivableView
{
    public string Description { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public DateOnly CreatedDate { get; init; }
    public DateOnly ReceivedDate { get; init; }
    public EReceivableStatus Status { get; init; }
};