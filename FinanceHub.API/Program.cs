using System.Text;
using FinanceHub.API.Filters;
using FinanceHub.Application.Services;
using FinanceHub.Application.TokenJWT;
using FinanceHub.Domain.DTOS.Input;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Security;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Domain.Validations.User;
using FinanceHub.Infrastructure.Data;
using FinanceHub.Infrastructure.Repositories;
using FinanceHub.Infrastructure.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVIÇOS ---

builder.Services.AddOpenApi();

// Pegamos a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

// Configuramos o DbContext para usar PostgreSQL
builder.Services.AddDbContext<FinanceHubDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("FinanceHub.Infrastructure")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPassWordHasher, PassWordHasher>();
builder.Services.AddScoped<ITokenJwt, TokenJwt>();
builder.Services.AddControllers(op => op.Filters.Add(typeof(ValidationFilter)));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<CreateUserValidation>();
});

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // em dev
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Você precisa estar logado para acessar este recurso."
                });
            },
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Token inválido ou expirado."
                });
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// --- 2. PIPELINE DE EXECUÇÃO (Middlewares) ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// --- 3. ENDPOINTS ---

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapControllers();

app.Run();

// --- 4. RECORDS / MODELS ---

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}