using FinanceHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHub.Infrastructure.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(x => x.SecondName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("second_name");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("email");

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("password");

        builder.Property(x => x.DateBirth)
            .HasColumnType("date")
            .HasColumnName("date_birth");

        builder.Property(x => x.Wallet)
            .HasPrecision(18, 2)
            .IsRequired(false)
            .HasColumnName("wallet");

        builder.Property(x => x.ConfirmEmail)
            .HasDefaultValue(false)
            .HasColumnName("confirm_email");

        builder.Property(x => x.VerifcationCode)
            .HasMaxLength(4)
            .IsRequired(false)
            .HasColumnName("verification_code");

        builder.Property(x => x.CodeExpiration)
            .IsRequired(false)
            .HasColumnName("code_expiration");

        builder.HasMany(x => x.Categories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}