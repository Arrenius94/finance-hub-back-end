namespace FinanceHub.Domain.DTOS.Output.AccountReceivable
{
    public sealed class DashBoardMetricsView
    {
        public int ReceivedCount { get; set; }
        public decimal ReceivedTotalValue { get; set; }
        public int PendingCount { get; set; }
        public decimal PendingTotalValue { get; set; }
        public int OverdueCount { get; set; }
        public decimal OverdueTotalValue { get; set; }
    };
}
