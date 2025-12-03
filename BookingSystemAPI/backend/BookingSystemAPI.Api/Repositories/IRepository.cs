using System.Linq.Expressions;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Interfaz genérica para repositorios con operaciones CRUD.
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad del dominio.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Obtiene todos los elementos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un elemento por su identificador.
    /// </summary>
    /// <param name="id">Identificador del elemento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Entidad encontrada o null.</returns>
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca elementos que cumplan con un predicado.
    /// </summary>
    /// <param name="predicate">Expresión de filtrado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de entidades que cumplen el predicado.</returns>
    Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el primer elemento que cumpla con un predicado.
    /// </summary>
    /// <param name="predicate">Expresión de filtrado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Primera entidad encontrada o null.</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva entidad.
    /// </summary>
    /// <param name="entity">Entidad a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Entidad agregada.</returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una entidad existente.
    /// </summary>
    /// <param name="entity">Entidad a actualizar.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Elimina una entidad.
    /// </summary>
    /// <param name="entity">Entidad a eliminar.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Verifica si existe algún elemento que cumpla con un predicado.
    /// </summary>
    /// <param name="predicate">Expresión de filtrado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe al menos un elemento.</returns>
    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene el conteo de elementos que cumplen con un predicado.
    /// </summary>
    /// <param name="predicate">Expresión de filtrado opcional.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Número de elementos.</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Guarda todos los cambios en la unidad de trabajo.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Número de entidades afectadas.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
