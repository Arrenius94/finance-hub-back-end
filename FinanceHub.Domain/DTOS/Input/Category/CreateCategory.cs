using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Category;

public class CreateCategory
{
    public string? Name { get; set; }
    public ECategoryType? CategoryType { get; set; }
}   