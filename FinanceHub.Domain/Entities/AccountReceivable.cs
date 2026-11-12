using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class AccountReceivable : BaseEntity
{
    public AccountReceivable(string description, decimal value, DateOnly receiveDate, EReceivableStatus status, int userId, Guid? installmentGroupId = null, int installmentNumber = 1, int totalInstallments = 1)
    {
        Description = description;
        Value = value;
        ReceiveDate = receiveDate;
        Status = status;
        UserId = userId;
        InstallmentGroupId = installmentGroupId;
        InstallmentNumber = installmentNumber;
        TotalInstallments = totalInstallments;
    }

    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public DateOnly ReceiveDate { get; private set; }
    public EReceivableStatus Status { get; private set; }

    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;
    public Guid? InstallmentGroupId { get; private set; }
    public int InstallmentNumber { get; private set; } = 1;
    public int TotalInstallments { get; private set; } = 1;

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