using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación del repositorio de Room.
/// </summary>
public class RoomRepository : Repository<Room>, IRoomRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de salas.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro.</param>
    public RoomRepository(ApplicationDbContext context, ILogger<Repository<Room>> logger) 
        : base(context, logger)
    {
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(r => r.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
