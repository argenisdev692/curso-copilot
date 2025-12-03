using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystemAPI.Api.Data.Configurations;

/// <summary>
/// Configuración de Fluent API para la entidad Booking.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    /// <summary>
    /// Configura el mapeo de la entidad Booking a la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // Nombre de la tabla
        builder.ToTable("Bookings");

        // Clave primaria
        builder.HasKey(b => b.Id);

        // Propiedades
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Título de la reserva");

        builder.Property(b => b.Description)
            .HasMaxLength(1000)
            .HasComment("Descripción detallada de la reserva");

        builder.Property(b => b.StartTime)
            .IsRequired()
            .HasComment("Fecha y hora de inicio (UTC)");

        builder.Property(b => b.EndTime)
            .IsRequired()
            .HasComment("Fecha y hora de fin (UTC)");

        builder.Property(b => b.OrganizerName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombre del organizador");

        builder.Property(b => b.OrganizerEmail)
            .IsRequired()
            .HasMaxLength(150)
            .HasComment("Email del organizador");

        builder.Property(b => b.AttendeeCount)
            .IsRequired()
            .HasComment("Número de asistentes esperados");

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(BookingStatus.Pending)
            .HasComment("Estado: 0=Pending, 1=Confirmed, 2=Cancelled, 3=Completed");

        builder.Property(b => b.Notes)
            .HasMaxLength(500)
            .HasComment("Notas adicionales");

        // Propiedades de auditoría (IAuditable)
        builder.Property(b => b.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.UpdatedAt);

        // Propiedades de soft delete (ISoftDelete)
        builder.Property(b => b.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(b => b.DeletedAt);

        // Relación con Room
        builder.HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(b => b.RoomId)
            .HasDatabaseName("IX_Bookings_RoomId");

        builder.HasIndex(b => b.StartTime)
            .HasDatabaseName("IX_Bookings_StartTime");

        builder.HasIndex(b => b.Status)
            .HasDatabaseName("IX_Bookings_Status");

        builder.HasIndex(b => b.OrganizerEmail)
            .HasDatabaseName("IX_Bookings_OrganizerEmail");

        // Índice compuesto para búsquedas de conflictos
        builder.HasIndex(b => new { b.RoomId, b.StartTime, b.EndTime })
            .HasDatabaseName("IX_Bookings_Room_TimeRange");

        // Global Query Filter para soft delete
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
