using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class Bill : BaseEntity
{
    public Bill(string description, decimal value, DateTime dateDue, DateTime? datePayment, EBillStatus billStatus, int categoryId)
    {
        Description = description;
        Value = value;
        DateDue = dateDue;
        DatePayment = datePayment;
        BillStatus = billStatus;
        CategoryId = categoryId;

        BillStatus = dateDue.Date < DateTime.UtcNow.Date
                     ? EBillStatus.Atrasado
                     : EBillStatus.Pendente;
    }

    public string Description { get; private set; }
    public decimal Value { get; private set; }
    public DateTime DateDue { get; private set; }
    public DateTime? DatePayment { get; private set; }
    public EBillStatus BillStatus { get; private set; }
    public int CategoryId { get; private set; }
    public virtual Category Category { get; private set; }
}