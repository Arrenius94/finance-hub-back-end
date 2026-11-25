using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("categories");
            
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(x => x.CategoryType)
            .IsRequired()
            .HasColumnName("category_type"); 

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.HasIndex(x => new { x.UserId, x.Name })
            .IsUnique()
            .HasDatabaseName("ux_categories_user_id_name");

        builder.HasOne(x => x.User)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}