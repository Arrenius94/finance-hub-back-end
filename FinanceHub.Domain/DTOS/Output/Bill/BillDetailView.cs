namespace FinanceHub.Domain.DTOS.Output.Bill;

public class BillDetailView
{
    public string Description { get; set; }
    public decimal TotalValue { get; set; }
    public int TotalInstallments { get; set; }
    public int PaidCount { get; set; }
    public List<BillView> Bills { get; set; }
}
