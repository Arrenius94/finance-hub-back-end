using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Bill;

public class BillQueryResult
{
    public EBillStatus Status { get; set; }
    public int Count { get; set; }
    public decimal Total { get; set; }
}