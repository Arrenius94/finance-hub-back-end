using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        {
            builder.ToTable("notification_logs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.NotificationType)
                .IsRequired()
                .HasColumnName("notification_type");

            builder.Property(x => x.SentAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("sent_at");

            builder.Property(x => x.BillId)
                .IsRequired()
                .HasColumnName("bill_id");

            builder.HasOne(x => x.Bill)
                .WithMany()
                .HasForeignKey(x => x.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.BillId, x.NotificationType }).IsUnique();
        }
    }
}