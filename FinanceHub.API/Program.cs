using FinanceHub.Infrastructure.Data; // Ajuste para o namespace onde está seu FinanceHubDbContext
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVIÇOS ---

builder.Services.AddOpenApi();

// Pegamos a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

// Configuramos o DbContext para usar PostgreSQL
builder.Services.AddDbContext<FinanceHubDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("FinanceHub.Infrastructure")));


var app = builder.Build();

// --- 2. PIPELINE DE EXECUÇÃO (Middlewares) ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// --- 3. ENDPOINTS ---

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

// --- 4. RECORDS / MODELS ---

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}