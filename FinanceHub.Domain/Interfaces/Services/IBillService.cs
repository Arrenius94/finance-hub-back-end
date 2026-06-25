using ErrorOr;
using FinanceHub.Domain.DTOS.Input.Bill;

namespace FinanceHub.Domain.Interfaces.Services;

public interface IBillService
{
    Task<ErrorOr<int>> CreateBillAsync (CreateBill request, int userId);
}