using ErrorOr;
using FinanceHub.Application.Common;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Input.Category;
using FinanceHub.Domain.DTOS.Output.Category;

namespace FinanceHub.Domain.Interfaces.Services;

public interface ICategoryService
{
    Task<ErrorOr<int>> CreateCategoryAsync(CreateCategory request);
    Task<ErrorOr<Success>> UpdateCategoryAsync(int categoryId, UpdateCategory request);
    Task<ErrorOr<PagedResult<CategoryView>>> GetAllAsync(CategoryFilter filter, CancellationToken ct);
    Task<ErrorOr<Success>> DeleteCategoryAsync(int categoryId);
    Task<ErrorOr<CategoryDetailView>> GetByIdAsync(int id, BillFilter filter, CancellationToken ct);
}