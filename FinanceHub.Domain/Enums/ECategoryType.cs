using System.ComponentModel;

namespace FinanceHub.Domain.Enums;

public enum ECategoryType
{
    [Description("Despesas")]
    Expense = 1,
    
    [Description("Receitas")]
    Revenue = 2,
}