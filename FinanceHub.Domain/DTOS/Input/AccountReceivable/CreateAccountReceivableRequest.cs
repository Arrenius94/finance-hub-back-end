using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.AccountReceivable;

public record CreateAccountReceivableRequest
(
        string Description, 
        decimal Value, 
        DateOnly ReceiveDate, 
        EReceivableStatus ReceiveStatus
);