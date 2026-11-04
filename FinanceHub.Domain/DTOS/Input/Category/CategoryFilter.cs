namespace FinanceHub.Domain.DTOS.Input.Category;

public class CategoryFilter
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
