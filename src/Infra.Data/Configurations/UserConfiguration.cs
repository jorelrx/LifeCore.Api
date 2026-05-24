using LifeOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOS.Infra.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FullName)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(180);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        // Google OAuth fields
        builder.Property(user => user.GoogleId)
            .HasMaxLength(200);

        builder.Property(user => user.GoogleAccessToken)
            .HasMaxLength(2000);

        builder.Property(user => user.GoogleRefreshToken)
            .HasMaxLength(2000);

        builder.Property(user => user.GoogleTokenExpiresAt);

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();
    }
}