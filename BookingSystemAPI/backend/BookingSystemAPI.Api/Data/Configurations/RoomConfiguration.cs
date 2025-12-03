using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystemAPI.Api.Data.Configurations;

/// <summary>
/// Configuración de Fluent API para la entidad Room.
/// </summary>
public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    /// <summary>
    /// Configura el mapeo de la entidad Room a la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de la entidad.</param>
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        // Nombre de la tabla
        builder.ToTable("Rooms");

        // Clave primaria
        builder.HasKey(r => r.Id);

        // Propiedades
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombre único de la sala");

        builder.Property(r => r.Capacity)
            .IsRequired()
            .HasComment("Capacidad máxima de personas");

        builder.Property(r => r.Equipment)
            .HasColumnType("nvarchar(max)")
            .HasComment("Lista de equipamiento en formato JSON");

        builder.Property(r => r.Location)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Ubicación física de la sala");

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(RoomStatus.Available)
            .HasComment("Estado de la sala: 0=Available, 1=Maintenance");

        // Propiedades de auditoría (IAuditable)
        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasComment("Fecha de creación del registro");

        builder.Property(r => r.UpdatedAt)
            .HasComment("Fecha de última actualización");

        // Propiedades de soft delete (ISoftDelete)
        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Indica si el registro está eliminado lógicamente");

        builder.Property(r => r.DeletedAt)
            .HasComment("Fecha de eliminación lógica");

        // Índice único en Name (solo para registros no eliminados)
        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("IX_Rooms_Name_Unique")
            .HasFilter("[IsDeleted] = 0");

        // Índice para consultas por Status
        builder.HasIndex(r => r.Status)
            .HasDatabaseName("IX_Rooms_Status");

        // Global Query Filter para soft delete
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
