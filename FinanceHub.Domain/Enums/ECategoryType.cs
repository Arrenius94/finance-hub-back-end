using System.ComponentModel;

namespace FinanceHub.Domain.Enums;

public enum ECategoryType
{
    [Description("Saúde")]
    Health = 1,
    
    [Description("Casa")]
    Home = 2,
    
    [Description("Lazer")]
    Leisure = 3,
    
    [Description("Mensalidades")]
    Subscriptions = 4
}