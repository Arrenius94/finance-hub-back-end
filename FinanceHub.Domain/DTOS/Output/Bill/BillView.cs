using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Output.Bill;

public class BillView
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime DateDue { get; set; }
    public DateTime? DatePayment { get; set; }
    public EBillStatus BillStatus { get; set; }
}
