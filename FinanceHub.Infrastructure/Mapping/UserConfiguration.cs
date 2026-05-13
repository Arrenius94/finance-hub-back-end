using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("Users")
            .HasKey(u => u.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.SecondName).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
        builder.Property(x => x.DateBirth).HasColumnType("date");
        
        builder.Property(x => x.Wallet).HasPrecision(18, 2);
        builder.Property(x => x.ConfirmEmail).HasDefaultValue(false);
        
        builder.HasMany(x => x.Categories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}