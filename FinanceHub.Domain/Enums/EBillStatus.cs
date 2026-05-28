using System.ComponentModel;

namespace FinanceHub.Domain.Enums;

public enum EBillStatus
{
    [Description("Pago")]
    Paid = 1,
    [Description("Pendente")]
    Pending = 2,
    [Description("Atrasado")]
    Overdue = 3
}