using System.Reflection;
using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Data;

public class FinanceHubDbContext : DbContext
{
    public FinanceHubDbContext(DbContextOptions<FinanceHubDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceHubDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}