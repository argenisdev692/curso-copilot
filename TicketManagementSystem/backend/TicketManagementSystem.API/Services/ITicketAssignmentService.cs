using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Servicio para la asignación de tickets a usuarios.
    /// </summary>
    public interface ITicketAssignmentService
    {
        /// <summary>
        /// Asigna un ticket a un usuario específico.
        /// </summary>
        /// <param name="ticketId">Identificador del ticket a asignar.</param>
        /// <param name="userId">Identificador del usuario al que asignar el ticket.</param>
        /// <param name="ct">Token de cancelación para operaciones asíncronas.</param>
        /// <returns>El ticket asignado.</returns>
        /// <exception cref="KeyNotFoundException">Se lanza cuando el ticket o usuario no existe.</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando el ticket está cerrado.</exception>
        /// <exception cref="UnauthorizedAccessException">Se lanza cuando el usuario no tiene rol de Agent.</exception>
        /// <example>
        /// var ticket = await _ticketAssignmentService.AssignTicketAsync(1, 2, CancellationToken.None);
        /// // Asignación exitosa
        /// </example>
        Task<Ticket> AssignTicketAsync(int ticketId, int userId, CancellationToken ct);

        /// <summary>
        /// Desasigna un ticket de su usuario actual.
        /// </summary>
        /// <param name="ticketId">Identificador del ticket a desasignar.</param>
        /// <param name="ct">Token de cancelación para operaciones asíncronas.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <example>
        /// var result = await _ticketAssignmentService.UnassignTicketAsync(1, CancellationToken.None);
        /// if (result.IsSuccess)
        /// {
        ///     // Desasignación exitosa
        /// }
        /// </example>
        Task<Result> UnassignTicketAsync(int ticketId, CancellationToken ct);

        /// <summary>
        /// Obtiene todos los tickets asignados a un usuario específico.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="ct">Token de cancelación para operaciones asíncronas.</param>
        /// <returns>Lista de tickets asignados al usuario.</returns>
        /// <example>
        /// var result = await _ticketAssignmentService.GetTicketsByUserAsync(2, CancellationToken.None);
        /// if (result.IsSuccess)
        /// {
        ///     var tickets = result.Value;
        ///     // Procesar tickets
        /// }
        /// else
        /// {
        ///     // Manejar error: result.Error
        /// }
        /// </example>
        Task<Result<List<Ticket>>> GetTicketsByUserAsync(int userId, CancellationToken ct);
    }
}