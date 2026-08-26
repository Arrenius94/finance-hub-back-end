using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.DTOS.Output.Bill;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Infrastructure.Security;

namespace FinanceHub.Application.Services;

public class BillService : IBillService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBillRepository _billRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    
    public BillService(
        ICategoryRepository categoryRepository,
        IBillRepository billRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _categoryRepository = categoryRepository;
        _billRepository = billRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }
    
    public async Task<ErrorOr<int>> CreateBillAsync(CreateBillRequest request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return AppErrors.Category.NotFound;
        
        var userId = _currentUser.UserId;
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return AppErrors.User.NotFound;
        
        if (category.UserId != userId)
            return AppErrors.Category.Unauthorized;
        
        var bill = new Bill(
            request.Description,
            request.Value,
            request.DateDue,
            request.DatePayment,
            request.CategoryId
        );

         _billRepository.Added(bill);

        if (bill.DatePayment.HasValue)
        {
            user.DecreaseValue(bill.Value);
            _userRepository.AttachForUpdate(user);
        }

        await _unitOfWork.CommitAsync(ct);
        
        return bill.Id;
    }

    public async Task<ErrorOr<DashboardMetricsView>> GetDashboardMetricsAsync(CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId < 0)
            return AppErrors.User.NotFound;
        
        var metrics = await _billRepository.GetThreeMetricsAsync(userId, ct);

        var dtoThreeMetrics = new DashboardMetricsView();

        foreach (var item in metrics)
        {
            switch (item.Status)
            {
                case EBillStatus.Paid:
                    dtoThreeMetrics.PaidCount = item.Count;
                    dtoThreeMetrics.PaidTotalValue = item.Total;
                    break;

                case EBillStatus.Pending:
                    dtoThreeMetrics.PendingCount = item.Count;
                    dtoThreeMetrics.PendingTotalValue = item.Total;
                    break;

                case EBillStatus.Overdue:
                    dtoThreeMetrics.OverdueCount = item.Count;
                    dtoThreeMetrics.OverdueTotalValue = item.Total;
                    break;
            }
        }

        return dtoThreeMetrics;
    }

    public async Task<ErrorOr<List<DashboardGraphicView>>> GetDashboardChartAsync(DashboarGraphicFilter filter, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId < 0)
            return AppErrors.User.NotFound;
        
        filter.UserId = userId;
        
        var result = await _billRepository.GetGraphicDataAsync(filter, ct);
        
        return result;
    }

    public async Task<ErrorOr<Success>> PayBillListAsync(PayBillsListRequest request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId < 0)
            return AppErrors.User.NotFound;
        
        if (request.BillIds.Count == 0)
            return AppErrors.Bill.NoBillsToPay;
        
        var bills = await _billRepository.GetByIdsPayment(request.BillIds, userId, ct);
        
        if(bills.Count < request.BillIds.Count)
            return AppErrors.Bill.DifferentList;
        
        var user = bills.First().Category.User;
        
        var totalAmount = bills.Sum(b => b.Value);
        if((user.Wallet ?? 0m) < totalAmount)
            return AppErrors.User.InsufficientBalance;
        
        user.DecreaseValue(totalAmount);

        foreach (var bill in bills)
        {
            bill.RegisterPayment();
        }
        
        await _unitOfWork.CommitAsync(ct);
        
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> DeleteBillAsync(DeleteBillsRequest request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if(userId < 0)
            return AppErrors.User.NotFound;
        
        if(request.billIds.Length == 0)
            return AppErrors.Bill.NoBillsToDelete;
        
        var bills = await _billRepository.GetByIdsDeleteAsync(request.billIds, userId, ct);
        
        if (bills.Count != request.billIds.Length)
            return AppErrors.Bill.DifferentList;
        
        _billRepository.RemoveRange(bills);
        
        await _unitOfWork.CommitAsync(ct);
        
        return Result.Success;
    }
}