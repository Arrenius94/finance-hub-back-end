using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Category;

public class UpdateCategory
{
    public string Name { get; set; }
    public ECategoryType CategoryType { get; set; }
}