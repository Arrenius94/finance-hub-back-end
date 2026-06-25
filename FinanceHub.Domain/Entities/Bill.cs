using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class Bill : BaseEntity
{
    public Bill(string description, decimal value, DateTime dateDue, DateTime? datePayment, int categoryId)
    {
        Description = description;
        Value = value;
        DateDue = dateDue;
        DatePayment = GetDatePayment(datePayment);
        CategoryId = categoryId;

       BillStatus = DatePayment.HasValue ? EBillStatus.Paid : EBillStatus.Pending;
    }

    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public DateTime DateDue { get; private set; }
    public DateTime? DatePayment { get; private set; }
    public EBillStatus BillStatus { get; private set; }
    public int CategoryId { get; private set; }
    public virtual Category Category { get; private set; }
    
    private static DateTime? GetDatePayment(DateTime? datePayment)
    {
        if (!datePayment.HasValue)
            return null;

        var payment = datePayment.Value;

        if (payment.TimeOfDay == TimeSpan.Zero)
        {
            var now = DateTime.Now;

            return payment.Date
                .AddHours(now.Hour)
                .AddMinutes(now.Minute)
                .AddSeconds(now.Second);
        }

        return payment;
    }
    
    public void RegisterPayment (DateTime datePayment)
    {
        DatePayment = DateTime.Now;
        BillStatus = EBillStatus.Paid;
    }
}