using System.ComponentModel;

namespace FinanceHub.Domain.Enums;

public enum ENotificationType
{
    [Description("SeteDias")]
    ReminderSevenDays = 1,
    [Description("TresDias")]
    ReminderThreeDays = 2,
    [Description("Hoje")]
    DueToday = 3
}