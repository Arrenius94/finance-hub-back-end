using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class AccountReceivable : BaseEntity
{
    public AccountReceivable(string description, decimal value, DateOnly receiveDate, EReceivableStatus status, int userId)
    {
        Description = description;
        Value = value;
        ReceiveDate = receiveDate;
        Status = status;
        UserId = userId;
    }
    
    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public DateOnly ReceiveDate { get; private set; }
    public EReceivableStatus Status { get; private set; }
    
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;
        
    public void MarkAsReceived()
    {
        Status = EReceivableStatus.Received;
    }

    public void UpdateAccountReceivable(string? description, decimal? value, DateOnly? receiveDate)
    {
        if (description is not null)
            Description = description;

        if (value.HasValue)
            Value = value.Value;

        if (receiveDate.HasValue)
            ReceiveDate = receiveDate.Value;
    }
}