using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Output.Bill;
using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Output.Category;

public class CategoryView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ECategoryType CategoryType { get; set; }
}

public class CategoryDetailView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ECategoryType CategoryType { get; set; }
    public PagedResult<BillView> Bills { get; set; }
}
