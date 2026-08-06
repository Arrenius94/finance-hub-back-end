namespace FinanceHub.Domain.DTOS.Input.Bill;

public class CreateBillRequest
{
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime DateDue { get; set; }
    public DateTime? DatePayment  { get; set; }
    public int CategoryId { get; set; }
}