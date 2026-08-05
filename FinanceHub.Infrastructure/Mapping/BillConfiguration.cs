using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder
            .ToTable("Bills")
            .HasKey(b => b.Id);

        builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        
        builder.Property(x => x.Value).IsRequired().HasPrecision(18, 2);
        
        builder.Property(x => x.DateDue).IsRequired().HasColumnType("date");
        builder.Property(x => x.DatePayment)
            .HasColumnType("timestamp with time zone");
        
        builder.Property(x => x.BillStatus).IsRequired();
        
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}