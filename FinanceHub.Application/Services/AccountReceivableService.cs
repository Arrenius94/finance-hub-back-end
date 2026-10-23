using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.DTOS.Output.AccountReceivable;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Infrastructure.Security;

namespace FinanceHub.Application.Services;

public class AccountReceivableService : IAccountReceivableService
{
    private readonly IAccountReceivableRepository _accountReceivableRepositoryrepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;


    public AccountReceivableService(IAccountReceivableRepository accountReceivableRepository, ICurrentUser currentUser, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _accountReceivableRepositoryrepository = accountReceivableRepository;
        _currentUser = currentUser;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<int>> CreateAccountReceivableServiceAsync(CreateAccountReceivableRequest request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return AppErrors.User.NotFound;
        }

        var accountReceivable = new AccountReceivable
        (
            request.Description,
            request.Value,
            request.ReceiveDate,
            request.ReceiveStatus,
            userId
        );

        await _accountReceivableRepositoryrepository.SaveAsync(accountReceivable, ct);

        return accountReceivable.Id;
    }

    public async Task<ErrorOr<AccountReceivableView>> GetAccountReceivableViewAsync(int id, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var accountReceivable = await _accountReceivableRepositoryrepository.GetByIdAsync(userId, id, ct);

        if (accountReceivable is null)
        {
            return AppErrors.AccountReceivable.NotFound;
        }

        var response = new AccountReceivableView
        {
            Description = accountReceivable.Description,
            Value = accountReceivable.Value,
            CreatedDate = DateOnly.FromDateTime(accountReceivable.CreatedAt),
            ReceivedDate = accountReceivable.ReceiveDate,
            Status = accountReceivable.Status
        };

        return response;
    }

    public async Task<ErrorOr<List<AccountReceivableView>>> GetAllReceivableViewAsync(AccountReceivableFilter request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var result = await _accountReceivableRepositoryrepository.GetAllAsync(userId, request, ct);

        return result;
    }

    public async Task<ErrorOr<Success>> UpdateAccountReceivableAsync(UpdateAccountReceivable request, int id, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var accountReceivable = await _accountReceivableRepositoryrepository.GetByIdAsync(userId, id, ct);

        if (accountReceivable is null)
        {
            return AppErrors.AccountReceivable.NotFound;
        }

        accountReceivable.UpdateAccountReceivable(request.Description, request.Value, request.ReceiveDate);

        await _accountReceivableRepositoryrepository.UpdateAsync(accountReceivable);

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> ReceivedListAsync(AccountReceivedListRequest request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        if (userId < 0) return AppErrors.AccountReceivable.NotFound;

        if (request.ReceivedList.Count == 0)
            return AppErrors.AccountReceivable.Void;

        var result = await _accountReceivableRepositoryrepository.GetByIdsAccountReceived(request.ReceivedList, userId, ct);

        if (result.Count == 0)
        {
            return AppErrors.AccountReceivable.NotFound;
        }

        if (result.Count < request.ReceivedList.Count)
        {
            return AppErrors.AccountReceivable.DifferentList;
        }

        var user = result.First().User;
        var total = result.Sum(a => a.Value);

        user.IncreaseValue(total);

        foreach (var r in result)
        {
            r.MarkAsReceived();
        }

        await _unitOfWork.CommitAsync(ct);

        return Result.Success;
    }

    public async Task<ErrorOr<DashBoardMetricsView>> GetThreeMetricsAsync(CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        if (userId < 0)
            return AppErrors.User.NotFound;

        var result = await _accountReceivableRepositoryrepository.GetThreeMetricsAsync(userId, ct);

        var dtoThreeMetrics = new DashBoardMetricsView();

        foreach (var item in result)
        {
            switch (item.Status)
            {
                case EReceivableStatus.Received:
                    dtoThreeMetrics.ReceivedCount = item.Count;
                    dtoThreeMetrics.ReceivedTotalValue = item.Total;
                    break;

                case EReceivableStatus.Pending:
                    dtoThreeMetrics.PendingCount = item.Count;
                    dtoThreeMetrics.PendingTotalValue = item.Total;
                    break;

                case EReceivableStatus.Overdue:
                    dtoThreeMetrics.OverdueCount = item.Count;
                    dtoThreeMetrics.OverdueTotalValue = item.Total;
                    break;
                
                default:
                    break;
            }
        }

        return dtoThreeMetrics;
    }

    public async Task<ErrorOr<List<DashBoardGraphicView>>> GetGraphicDataAsync(DashBoardGraphicFilter filter, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        if (userId < 0)
            return AppErrors.User.NotFound;

        var result = await _accountReceivableRepositoryrepository.GetGraphicDataAsync(filter, ct);

        return result;
    }
}