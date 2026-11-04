using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Bill;

public class BillFilter
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EBillStatus? BillStatus { get; set; }
}
