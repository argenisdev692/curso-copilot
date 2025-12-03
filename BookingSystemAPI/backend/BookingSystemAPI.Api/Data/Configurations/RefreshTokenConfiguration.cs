using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystemAPI.Api.Data.Configurations;

/// <summary>
/// Configuración de Fluent API para la entidad RefreshToken.
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.IsRevoked)
            .HasDefaultValue(false);

        builder.Property(rt => rt.CreatedByIp)
            .HasMaxLength(45);

        // Índice único para el token
        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        // Índice para búsquedas por usuario
        builder.HasIndex(rt => rt.UserId);

        // Relación con User
        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
