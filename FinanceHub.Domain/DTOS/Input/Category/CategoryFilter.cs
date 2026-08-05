using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Category;

public class CategoryFilter
{
    public int? RestrictUserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EBillStatus? BillStatus { get; set; } 
}