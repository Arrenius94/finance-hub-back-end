using ErrorOr;
using FinanceHub.Application.Errors;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.Entities;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;

namespace FinanceHub.Application.Services;

public class BillService : IBillService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBillRepository _billRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public BillService(
        ICategoryRepository categoryRepository,
        IBillRepository billRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _billRepository = billRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<int>> CreateBillAsync(CreateBill request, int  userId)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return AppErrors.Category.NotFound;
        
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
            _userRepository.Update(user);
        }

        await _unitOfWork.CommitAsync();
        
        return bill.Id;
    }
}