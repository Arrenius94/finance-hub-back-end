using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Bill;

public class DashboardChartFilter
{
    public int UserId { get; set; }
    public int? Month  { get; set; }
    public int? Year { get; set; }
    
    public List<ECategoryType>? CategoryTypes { get; set; }
    
    
}