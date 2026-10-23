using System.ComponentModel;

namespace FinanceHub.Domain.Enums;

public enum EReceivableStatus
{
    [Description("Recebido")]
    Received = 1,
    [Description("Pendente")]
    Pending = 2,
    [Description("Atrasado")]
    Overdue = 3,
}