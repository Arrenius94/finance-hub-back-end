using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class NotificationLog
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public Bill Bill { get; set; } = null!;
    public ENotificationType NotificationType { get; set; }
    public DateTime SentAt { get; set; }
}