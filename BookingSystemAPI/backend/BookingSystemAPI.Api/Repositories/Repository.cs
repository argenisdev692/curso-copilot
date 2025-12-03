using System.Linq.Expressions;
using BookingSystemAPI.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación genérica del repositorio con Entity Framework Core.
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad del dominio.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Contexto de base de datos.
    /// </summary>
    protected readonly ApplicationDbContext _context;

    /// <summary>
    /// DbSet de la entidad.
    /// </summary>
    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Logger para registro de operaciones.
    /// </summary>
    protected readonly ILogger<Repository<TEntity>> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro.</param>
    public Repository(ApplicationDbContext context, ILogger<Repository<TEntity>> logger)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _logger = logger;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Obteniendo todos los elementos de {EntityType}", typeof(TEntity).Name);
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Buscando {EntityType} con Id: {Id}", typeof(TEntity).Name, id);
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Buscando elementos de {EntityType} con predicado", typeof(TEntity).Name);
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Agregando nuevo {EntityType}", typeof(TEntity).Name);
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public virtual void Update(TEntity entity)
    {
        _logger.LogDebug("Actualizando {EntityType}", typeof(TEntity).Name);
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    /// <inheritdoc/>
    public virtual void Delete(TEntity entity)
    {
        _logger.LogDebug("Eliminando {EntityType}", typeof(TEntity).Name);
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return predicate == null
            ? await _dbSet.CountAsync(cancellationToken)
            : await _dbSet.CountAsync(predicate, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Guardando cambios en la base de datos");
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
