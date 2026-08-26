using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Infrastructure.Data;
using FinanceHub.Infrastructure.Repositories; // Ajuste o namespace para o seu DbContext se for diferente
using FinanceHub.Infrastructure.Services;
using FinanceHub.Worker;
using Microsoft.EntityFrameworkCore;
using Resend;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<FinanceHubDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddResend(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<INotificationLogRepository, NotificationLogRepository>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();
// Adicione aqui a injeção dos repositórios que o Worker utilizar (ex: IBillRepository, etc)

// 4. Registra o serviço em segundo plano (Worker)
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();