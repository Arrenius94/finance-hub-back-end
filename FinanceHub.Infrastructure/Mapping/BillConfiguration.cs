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

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(x => x.CategoryId)
            .IsRequired()
            .HasColumnName("category_id");

        builder.Property(x => x.InstallmentGroupId)
            .HasColumnName("installment_group_id")
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.HasIndex(x => x.InstallmentGroupId)
            .HasDatabaseName("ix_bills_installment_group_id");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_bills_user_id");

        builder.Property(x => x.InstallmentNumber)
            .HasColumnName("installment_number")
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(x => x.TotalInstallments)
            .HasColumnName("total_installments")
            .IsRequired()
            .HasDefaultValue(1);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Bills)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
