using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interfaz para el servicio de gestión de prioridades.
    /// Define las operaciones CRUD y consultas para prioridades.
    /// </summary>
    public interface IPriorityService
    {
        /// <summary>
        /// Obtiene una lista paginada de prioridades según los parámetros de consulta.
        /// </summary>
        /// <param name="parameters">Parámetros de consulta para filtrar y paginar.</param>
        /// <returns>Respuesta paginada con las prioridades.</returns>
        Task<PagedResponse<PriorityDto>> GetPrioritiesAsync(GetPrioritiesQueryParameters parameters);

        /// <summary>
        /// Obtiene una prioridad por su ID.
        /// </summary>
        /// <param name="id">ID de la prioridad.</param>
        /// <returns>DTO de la prioridad si existe, null en caso contrario.</returns>
        Task<PriorityDto?> GetPriorityByIdAsync(int id);

        /// <summary>
        /// Crea una nueva prioridad.
        /// </summary>
        /// <param name="dto">Datos para crear la prioridad.</param>
        /// <returns>DTO de la prioridad creada.</returns>
        Task<PriorityDto> CreatePriorityAsync(CreatePriorityDto dto);

        /// <summary>
        /// Actualiza una prioridad existente.
        /// </summary>
        /// <param name="id">ID de la prioridad a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>DTO de la prioridad actualizada.</returns>
        Task<PriorityDto> UpdatePriorityAsync(int id, UpdatePriorityDto dto);

        /// <summary>
        /// Elimina una prioridad (soft delete).
        /// </summary>
        /// <param name="id">ID de la prioridad a eliminar.</param>
        Task DeletePriorityAsync(int id);

        /// <summary>
        /// Activa o desactiva una prioridad.
        /// </summary>
        /// <param name="id">ID de la prioridad.</param>
        /// <param name="isActive">Estado activo/inactivo.</param>
        Task TogglePriorityActiveAsync(int id, bool isActive);

        /// <summary>
        /// Obtiene todas las prioridades activas ordenadas por nivel.
        /// </summary>
        /// <returns>Lista de prioridades activas.</returns>
        Task<List<PriorityDto>> GetActivePrioritiesAsync();

        /// <summary>
        /// Verifica si existe una prioridad con el nombre especificado (excluyendo el ID dado).
        /// </summary>
        /// <param name="name">Nombre a verificar.</param>
        /// <param name="excludeId">ID a excluir de la verificación.</param>
        /// <returns>True si existe, false en caso contrario.</returns>
        Task<bool> PriorityNameExistsAsync(string name, int? excludeId = null);

        /// <summary>
        /// Verifica si existe una prioridad con el nivel especificado (excluyendo el ID dado).
        /// </summary>
        /// <param name="level">Nivel a verificar.</param>
        /// <param name="excludeId">ID a excluir de la verificación.</param>
        /// <returns>True si existe, false en caso contrario.</returns>
        Task<bool> PriorityLevelExistsAsync(int level, int? excludeId = null);
    }
}