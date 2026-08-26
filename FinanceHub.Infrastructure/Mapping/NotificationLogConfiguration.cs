using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        {
            builder.ToTable("NotificationLogs")
                .HasKey(x => x.Id);

            builder.Property(x => x.NotificationType)
                .IsRequired();

            builder.Property(x => x.SentAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.HasOne(x => x.Bill)
                .WithMany()
                .HasForeignKey(x => x.BillId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(x => new { x.BillId, x.NotificationType }).IsUnique();
        }
    }
}