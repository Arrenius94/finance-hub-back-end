using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.DTOS.Input.Category;

public sealed record UpdateCategory(string Name, ECategoryType? CategoryType);