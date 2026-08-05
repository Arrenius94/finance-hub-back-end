namespace FinanceHub.Domain.DTOS.Output.Bill;

public class DashboardMetricsView
{
    public int PaidCount { get; set; }
    public decimal PaidTotalValue { get; set; }
    public int PendingCount { get; set; }
    public decimal PendingTotalValue { get; set; }
    public int OverdueCount { get; set; }
    public decimal OverdueTotalValue { get; set; }
}