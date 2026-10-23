using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.AccountReceivable;

public record AccountReceivableFilter(string? Description, DateOnly? ReceiveDate, EReceivableStatus? ReceivableStatus);