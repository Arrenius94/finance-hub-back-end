using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Output.AccountReceivable;

public record AccountReceivableView
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public DateOnly CreatedDate { get; init; }
    public DateOnly ReceivedDate { get; init; }
    public EReceivableStatus Status { get; init; }
    public int InstallmentCount { get; init; }
    public int TotalInstallments { get; init; }
};

public record AccountReceivableDetailView
{
    public string Description { get; init; } = string.Empty;
    public decimal TotalValue { get; init; }
    public int TotalInstallments { get; init; }
    public int ReceivedCount { get; init; }
    public List<AccountReceivableView> Installments { get; init; } = [];
}