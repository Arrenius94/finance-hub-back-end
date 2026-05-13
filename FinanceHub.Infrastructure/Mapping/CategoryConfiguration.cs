using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("Categories")
            .HasKey(c => c.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CategoryType).IsRequired();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Bills)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}