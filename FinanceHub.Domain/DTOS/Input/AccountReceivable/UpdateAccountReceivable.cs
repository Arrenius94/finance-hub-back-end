namespace FinanceHub.Domain.DTOS.Input.AccountReceivable;

public sealed record UpdateAccountReceivable(string? Description, decimal? Value, DateOnly? ReceiveDate);