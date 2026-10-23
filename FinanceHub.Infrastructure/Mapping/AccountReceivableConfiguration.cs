using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class AccountReceivableConfiguration : IEntityTypeConfiguration<AccountReceivable>
{
    public void Configure(EntityTypeBuilder<AccountReceivable> builder)
    {
        builder.ToTable("accounts_receivable");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("description");

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("value"); 

        builder.Property(x => x.ReceiveDate)
            .IsRequired()
            .HasColumnName("receive_date")
            .HasColumnType("date");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasColumnName("status"); 

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        
        builder.HasOne(r => r.User)
            .WithMany(u => u.AccountReceivables)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}