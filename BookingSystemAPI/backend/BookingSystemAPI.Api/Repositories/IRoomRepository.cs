using BookingSystemAPI.Api.Models;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Repositorio específico para operaciones de Room.
/// </summary>
public interface IRoomRepository : IRepository<Room>
{
    /// <summary>
    /// Verifica si existe una sala con el nombre especificado.
    /// </summary>
    /// <param name="name">Nombre de la sala.</param>
    /// <param name="excludeId">ID a excluir de la búsqueda (para updates).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe, False en caso contrario.</returns>
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
}
