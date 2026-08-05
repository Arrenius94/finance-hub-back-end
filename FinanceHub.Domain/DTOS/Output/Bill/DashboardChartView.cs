namespace FinanceHub.Domain.DTOS.Output.Bill;

public class DashboardChartView
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
}