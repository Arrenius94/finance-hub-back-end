namespace FinanceHub.Domain.DTOS.Input.Bill;

public sealed record DashboarGraphicFilter
(
    int UserId,
    int? Month = null,
    int? Year = null,
    List<int>? CategoryIds = null
);