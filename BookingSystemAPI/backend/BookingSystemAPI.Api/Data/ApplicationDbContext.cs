using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Api.Data;

/// <summary>
/// Contexto principal de Entity Framework Core para la aplicación.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia del contexto de base de datos.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Conjunto de salas del sistema.
    /// </summary>
    public DbSet<Room> Rooms => Set<Room>();

    /// <summary>
    /// Conjunto de reservas del sistema.
    /// </summary>
    public DbSet<Booking> Bookings => Set<Booking>();

    /// <summary>
    /// Conjunto de usuarios del sistema.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Conjunto de tokens de refresco del sistema.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// Configura el modelo de datos utilizando Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones de entidades del ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configurar comportamiento de eliminación por defecto
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    /// <summary>
    /// Guarda los cambios y aplica auditoría automática.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Número de entidades afectadas.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Aplica información de auditoría a las entidades modificadas.
    /// </summary>
    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Models.IAuditable &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var auditable = (Models.IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }

            auditable.UpdatedAt = DateTime.UtcNow;
        }
    }
}
