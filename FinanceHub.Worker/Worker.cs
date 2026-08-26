using System.Text;
using FinanceHub.Domain.Enums;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;

namespace FinanceHub.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;
    
    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       _logger.LogInformation("Worker de Notificações de Finanças iniciado.");

       while (!stoppingToken.IsCancellationRequested)
       {
           try
           {
                await ProcessPendingNotificationsAsync(stoppingToken);
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               throw;
           }
           await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
       }
    }

    private async Task ProcessPendingNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var billRepository = scope.ServiceProvider.GetRequiredService<IBillRepository>();
        var notificationLogRepository = scope.ServiceProvider.GetRequiredService<INotificationLogRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var today = DateTime.UtcNow.Date;
        var pendingBills = await billRepository.GetPendingBillsForNotificationAsync(today, cancellationToken);
        
        if (pendingBills.Count <= 0) return;
        
        var groupedBills = pendingBills
            .Select(bill => new
            {
                Bill = bill,
                Type = (bill.DateDue.Date - today).Days switch
                {
                    7 => (ENotificationType?)ENotificationType.ReminderSevenDays,
                    3 => (ENotificationType?)ENotificationType.ReminderThreeDays,
                    0 => (ENotificationType?)ENotificationType.DueToday,
                    _ => null
                }
            })
            .Where(x => x.Type != null)
            .GroupBy(x => new { UserId = x.Bill.Category.UserId, Type = x.Type!.Value });

        
        // PERCORER CADA GRUPO DO USUARIO COM O TIPO DAS CONSTAS ENOTFCATION E AS LISTAS DOS IDS DAS CONSTAS E VER QUAL DOS IDS DAS BILLS JA FOI NOTIFICADA
        foreach (var group in groupedBills)
        {
            var notificationType = group.Key.Type;
            var groupBillIds = group.Select(x => x.Bill.Id).ToList();
            
            var alreadyNotifiedIds = await notificationLogRepository.GetNotificationBillsIdsTodayAsync(groupBillIds, notificationType, today, cancellationToken);
            
            // trouxe ids das listas que foram notificadas, agr eu filtro essa lista com os ids q nao foram notificados
            var billsToNotify = group
                .Where(x => !alreadyNotifiedIds.Contains(x.Bill.Id))
                .Select(x => x.Bill)
                .ToList();

            if (billsToNotify.Count <= 0) continue;

            var user = billsToNotify.First().Category.User;

            var (subject, textDetail) = notificationType switch
            {
                ENotificationType.ReminderSevenDays => (
                    $"[FinanceHub] Lembrete: Você tem contas que vencem em 7 dias",
                    "falta <strong>7 dias</strong> para o vencimento:"
                ),
                ENotificationType.ReminderThreeDays => (
                    $"[FinanceHub] Lembrete: Você tem contas que vencem em 3 dias",
                    "falta <strong>3 dias</strong> para o vencimento:"
                ),
                ENotificationType.DueToday => (
                    $"[FinanceHub] Atenção: Você tem contas que vencem HOJE!",
                    "<strong>vencem hoje</strong>:"
                ),
                _ => (string.Empty, string.Empty)
            };
            
            // montar a lista das contas com descrição e data
            var billsListHtml = new StringBuilder();
            billsListHtml.Append("<ul style='line-height: 1.6;'>");

            foreach (var bill in billsToNotify)
            { 
               billsListHtml.Append($"<li><strong>{bill.Description}</strong> - R$ {bill.Value:N2} (Vencimento: {bill.DateDue:dd/MM/yyyy})</li>");
            }
            
            billsListHtml.Append("</ul>");
            
            var body = $"""
                        <h2>Olá, {user.Name}!</h2>
                        <p>Este é um aviso automático do seu <strong>FinanceHub</strong>.</p>
                        <p>As seguintes contas {textDetail}</p>
                        {billsListHtml}
                        <br/>
                        <p>Acesse o sistema para conferir ou atualizar o status de pagamento.</p>
                        """;

            await emailService.SendEmailAsync(user.Email, subject, body);

            // add log nas contas q foram notificads
            foreach (var bill in billsToNotify)
            {
                await notificationLogRepository.AddLogAsync(bill.Id, notificationType, cancellationToken);
            }
            
            var billNames = string.Join(", ", billsToNotify.Select(b => b.Description));

            _logger.LogInformation(
                "E-mail [{Type}] enviado para {Email} com {Count} conta(s): [{Bills}]",
                notificationType,
                user.Email,
                billsToNotify.Count,
                billNames
            );
        }
        
    }
}