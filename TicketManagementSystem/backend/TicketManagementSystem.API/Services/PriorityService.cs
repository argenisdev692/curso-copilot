using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Servicio para la gestión de prioridades.
    /// Implementa operaciones CRUD optimizadas con validaciones y logging.
    /// </summary>
    public class PriorityService : IPriorityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PriorityService> _logger;

        /// <summary>
        /// Constructor del servicio de prioridades.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registro de operaciones.</param>
        public PriorityService(ApplicationDbContext context, ILogger<PriorityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista paginada de prioridades.
        /// </summary>
        public async Task<PagedResponse<PriorityDto>> GetPrioritiesAsync(GetPrioritiesQueryParameters parameters)
        {
            var query = _context.Priorities.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(p => p.Name.Contains(parameters.Name));
            }

            if (parameters.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == parameters.IsActive.Value);
            }

            // Contar total antes de paginar
            var totalItems = await query.CountAsync();

            // Aplicar ordenamiento
            query = parameters.SortBy?.ToLower() switch
            {
                "name" => parameters.SortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "level" => parameters.SortOrder == "desc" ? query.OrderByDescending(p => p.Level) : query.OrderBy(p => p.Level),
                "createdat" => parameters.SortOrder == "desc" ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => query.OrderBy(p => p.Level)
            };

            // Aplicar paginación
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(p => new PriorityDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Level = p.Level,
                    Description = p.Description,
                    Color = p.Color,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / parameters.PageSize);

            return new PagedResponse<PriorityDto>
            {
                Items = items,
                TotalItems = totalItems,
                Page = parameters.Page,
                PageSize = parameters.PageSize,
                TotalPages = totalPages,
                HasNextPage = parameters.Page < totalPages,
                HasPreviousPage = parameters.Page > 1
            };
        }

        /// <summary>
        /// Obtiene una prioridad por ID.
        /// </summary>
        public async Task<PriorityDto?> GetPriorityByIdAsync(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null)
            {
                return null;
            }

            return new PriorityDto
            {
                Id = priority.Id,
                Name = priority.Name,
                Level = priority.Level,
                Description = priority.Description,
                Color = priority.Color,
                IsActive = priority.IsActive,
                CreatedAt = priority.CreatedAt,
                UpdatedAt = priority.UpdatedAt
            };
        }

        /// <summary>
        /// Crea una nueva prioridad.
        /// </summary>
        public async Task<PriorityDto> CreatePriorityAsync(CreatePriorityDto dto)
        {
            // Validar unicidad
            if (await PriorityNameExistsAsync(dto.Name))
            {
                throw new InvalidOperationException($"Ya existe una prioridad con el nombre '{dto.Name}'.");
            }

            if (await PriorityLevelExistsAsync(dto.Level))
            {
                throw new InvalidOperationException($"Ya existe una prioridad con el nivel {dto.Level}.");
            }

            var priority = new Priority
            {
                Name = dto.Name,
                Level = dto.Level,
                Description = dto.Description,
                Color = dto.Color,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Priorities.Add(priority);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Prioridad creada: {Name} (ID: {Id})", priority.Name, priority.Id);

            return await GetPriorityByIdAsync(priority.Id) ?? throw new InvalidOperationException("Error al crear la prioridad.");
        }

        /// <summary>
        /// Actualiza una prioridad existente.
        /// </summary>
        public async Task<PriorityDto> UpdatePriorityAsync(int id, UpdatePriorityDto dto)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null)
            {
                throw new KeyNotFoundException($"No se encontró la prioridad con ID {id}.");
            }

            // Validar unicidad
            if (await PriorityNameExistsAsync(dto.Name, id))
            {
                throw new InvalidOperationException($"Ya existe otra prioridad con el nombre '{dto.Name}'.");
            }

            if (await PriorityLevelExistsAsync(dto.Level, id))
            {
                throw new InvalidOperationException($"Ya existe otra prioridad con el nivel {dto.Level}.");
            }

            priority.Name = dto.Name;
            priority.Level = dto.Level;
            priority.Description = dto.Description;
            priority.Color = dto.Color;
            priority.IsActive = dto.IsActive;
            priority.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Prioridad actualizada: {Name} (ID: {Id})", priority.Name, priority.Id);

            return await GetPriorityByIdAsync(id) ?? throw new InvalidOperationException("Error al actualizar la prioridad.");
        }

        /// <summary>
        /// Elimina una prioridad (soft delete).
        /// </summary>
        public async Task DeletePriorityAsync(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null)
            {
                throw new KeyNotFoundException($"No se encontró la prioridad con ID {id}.");
            }

            // Verificar si hay tickets usando esta prioridad
            var hasTickets = await _context.Tickets.AnyAsync(t => t.PriorityId == id);
            if (hasTickets)
            {
                throw new InvalidOperationException("No se puede eliminar la prioridad porque está siendo utilizada por tickets existentes.");
            }

            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Prioridad eliminada: {Name} (ID: {Id})", priority.Name, priority.Id);
        }

        /// <summary>
        /// Activa o desactiva una prioridad.
        /// </summary>
        public async Task TogglePriorityActiveAsync(int id, bool isActive)
        {
            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null)
            {
                throw new KeyNotFoundException($"No se encontró la prioridad con ID {id}.");
            }

            priority.IsActive = isActive;
            priority.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Prioridad {Action}: {Name} (ID: {Id})", isActive ? "activada" : "desactivada", priority.Name, priority.Id);
        }

        /// <summary>
        /// Obtiene todas las prioridades activas.
        /// </summary>
        public async Task<List<PriorityDto>> GetActivePrioritiesAsync()
        {
            return await _context.Priorities
                .Where(p => p.IsActive)
                .OrderBy(p => p.Level)
                .Select(p => new PriorityDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Level = p.Level,
                    Description = p.Description,
                    Color = p.Color,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe una prioridad con el nombre especificado.
        /// </summary>
        public async Task<bool> PriorityNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.Priorities.Where(p => p.Name == name);
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        /// <summary>
        /// Verifica si existe una prioridad con el nivel especificado.
        /// </summary>
        public async Task<bool> PriorityLevelExistsAsync(int level, int? excludeId = null)
        {
            var query = _context.Priorities.Where(p => p.Level == level);
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }
    }
}