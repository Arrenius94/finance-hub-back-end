

namespace FinanceHub.Domain.DTOS.Input.Bill;

public sealed record UpdateBill(
    string? Description,
    decimal? Value,
    DateTime? DateDue,
    int? CategoryId
);