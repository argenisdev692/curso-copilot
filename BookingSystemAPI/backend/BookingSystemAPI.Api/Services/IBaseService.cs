namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Interfaz base para servicios de la aplicación.
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad del dominio.</typeparam>
/// <typeparam name="TDto">Tipo de DTO de respuesta.</typeparam>
/// <typeparam name="TCreateDto">Tipo de DTO para creación.</typeparam>
/// <typeparam name="TUpdateDto">Tipo de DTO para actualización.</typeparam>
public interface IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : class
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    /// <summary>
    /// Obtiene todos los elementos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de DTOs.</returns>
    Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un elemento por su identificador.
    /// </summary>
    /// <param name="id">Identificador del elemento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del elemento encontrado.</returns>
    Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo elemento.
    /// </summary>
    /// <param name="createDto">DTO con los datos de creación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del elemento creado.</returns>
    Task<TDto> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un elemento existente.
    /// </summary>
    /// <param name="id">Identificador del elemento.</param>
    /// <param name="updateDto">DTO con los datos de actualización.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO del elemento actualizado.</returns>
    Task<TDto> UpdateAsync(int id, TUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina un elemento (soft delete).
    /// </summary>
    /// <param name="id">Identificador del elemento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si se eliminó correctamente.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
