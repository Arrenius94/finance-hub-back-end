using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class Bill : BaseEntity
{
    public Bill(
        string description,
        decimal value,
        DateTime dateDue,
        DateTime? datePayment,
        int userId,
        int categoryId,
        Guid? installmentGroupId = null,
        int installmentNumber = 1,
        int totalInstallments = 1)
    {
        Description = description;
        Value = value;
        DateDue = dateDue;
        DatePayment = GetDatePayment(datePayment);
        UserId = userId;
        CategoryId = categoryId;
        InstallmentGroupId = installmentGroupId;
        InstallmentNumber = installmentNumber;
        TotalInstallments = totalInstallments;

        BillStatus = DatePayment.HasValue ? EBillStatus.Paid : EBillStatus.Pending;
    }

    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public DateTime DateDue { get; private set; }
    public DateTime? DatePayment { get; private set; }
    public EBillStatus BillStatus { get; private set; }

    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public int CategoryId { get; private set; }
    public virtual Category Category { get; private set; } = null!;

    public Guid? InstallmentGroupId { get; private set; }
    public int InstallmentNumber { get; private set; } = 1;
    public int TotalInstallments { get; private set; } = 1;

    private static DateTime? GetDatePayment(DateTime? datePayment)
    {
        if (!datePayment.HasValue)
            return null;

        var payment = datePayment.Value;

        if (payment.TimeOfDay == TimeSpan.Zero)
        {
            var now = DateTime.UtcNow;

            payment = payment.Date
                .AddHours(now.Hour)
                .AddMinutes(now.Minute)
                .AddSeconds(now.Second);
        }

        return DateTime.SpecifyKind(payment, DateTimeKind.Utc);
    }

    public void RegisterPayment()
    {
        DatePayment = DateTime.UtcNow;
        BillStatus = EBillStatus.Paid;
    }

    public void UpdateBill(string? description, decimal? value, DateTime? dateDue, int? categoryId)
    {
        if (description is not null)
            Description = description;

        if (value.HasValue)
            Value = value.Value;

        if (dateDue.HasValue)
            DateDue = dateDue.Value;

        if (categoryId.HasValue)
            CategoryId = categoryId.Value;
    }
}