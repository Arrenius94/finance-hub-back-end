using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
    
        builder.ToTable("bills");
        
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("description");

        builder.Property(x => x.Value)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasColumnName("value");

        builder.Property(x => x.DateDue)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName("date_due");

        builder.Property(x => x.DatePayment)
            .HasColumnType("timestamp with time zone")
            .HasColumnName("date_payment");

        builder.Property(x => x.BillStatus)
            .IsRequired()
            .HasColumnName("bill_status");

        builder.Property(x => x.CategoryId)
            .IsRequired()
            .HasColumnName("category_id");
        
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}