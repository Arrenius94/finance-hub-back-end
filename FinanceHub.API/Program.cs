using System.Text;
using FinanceHub.API.Auth;
using FinanceHub.API.Filters;
using FinanceHub.Application.Services;
using FinanceHub.Application.TokenJWT;
using FinanceHub.Domain.Interfaces.Repositories;
using FinanceHub.Domain.Interfaces.Security;
using FinanceHub.Domain.Interfaces.Services;
using FinanceHub.Domain.Validations.User;
using FinanceHub.Infrastructure.Data;
using FinanceHub.Infrastructure.Repositories;
using FinanceHub.Infrastructure.Security;
using FinanceHub.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Resend;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVIÇOS ---

// Configuração do OpenAPI com Suporte a Bearer Token no Scalar
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Garante que Components e SecuritySchemes não estejam nulos
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // 2. Instancia o esquema Bearer
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Cole aqui o token JWT retornado no endpoint de Login."
        };

        // 3. Adiciona o esquema com segurança sem estourar NullReferenceException
        document.Components.SecuritySchemes["Bearer"] = scheme;

        // 4. Garante que a lista de requisitos de segurança não esteja nula
        var requirement = new OpenApiSecurityRequirement();
        var schemeRef = new OpenApiSecuritySchemeReference("Bearer", document);
        
        requirement.Add(schemeRef, new List<string>());
        
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(requirement);

        return Task.CompletedTask;
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();

// Resend Configuration
builder.Services.AddHttpClient<IResend, ResendClient>();
builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<FinanceHubDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("FinanceHub.Infrastructure")));

// Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IAccountReceivableService, AccountReceivableService>();
builder.Services.AddScoped<IAccountReceivableRepository, AccountReceivableRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPassWordHasher, PassWordHasher>();
builder.Services.AddScoped<ITokenJwt, TokenJwt>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEmailService, ResendEmailService>();

// Controllers & Validation
builder.Services.AddControllers(op => op.Filters.Add(typeof(ValidationFilter)));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<CreateUserValidation>();
});

// JWT Authentication
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

// --- 2. PIPELINE DE EXECUÇÃO ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("FinanceHub API")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDarkMode(true)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// --- 3. ENDPOINTS ---

app.MapControllers();

app.Run();