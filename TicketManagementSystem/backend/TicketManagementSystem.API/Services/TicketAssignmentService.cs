using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Servicio para la asignación de tickets a usuarios.
    /// Implementa lógica de negocio para asignar y desasignar tickets.
    /// </summary>
    public class TicketAssignmentService : BaseService, ITicketAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketRepository _ticketRepository;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del servicio.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo para acceso a datos.</param>
        /// <param name="ticketRepository">Repositorio de tickets.</param>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public TicketAssignmentService(
            IUnitOfWork unitOfWork,
            ITicketRepository ticketRepository,
            ApplicationDbContext context,
            ILogger<TicketAssignmentService> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
            _context = context;
        }

        /// <summary>
        /// Asigna un ticket a un usuario específico.
        /// </summary>
        /// <param name="ticketId">Identificador del ticket a asignar.</param>
        /// <param name="userId">Identificador del usuario al que asignar el ticket.</param>
        /// <param name="ct">Token de cancelación para operaciones asíncronas.</param>
        /// <returns>El ticket asignado.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando los identificadores son inválidos.</exception>
        /// <exception cref="KeyNotFoundException">Se lanza cuando el ticket o usuario no existe.</exception>
        /// <exception cref="InvalidOperationException">Se lanza cuando el ticket está cerrado.</exception>
        /// <exception cref="UnauthorizedAccessException">Se lanza cuando el usuario no tiene rol de Agent.</exception>
        /// <example>
        /// var ticket = await _ticketAssignmentService.AssignTicketAsync(1, 2, CancellationToken.None);
        /// // Asignación exitosa
        /// </example>
        public async Task<Ticket> AssignTicketAsync(int ticketId, int userId, CancellationToken ct)
        {
            // Guard clauses
            if (ticketId <= 0)
                throw new ArgumentException("El identificador del ticket debe ser mayor a cero.", nameof(ticketId));

            if (userId <= 0)
                throw new ArgumentException("El identificador del usuario debe ser mayor a cero.", nameof(userId));

            ct.ThrowIfCancellationRequested();

            LogInformation("Iniciando asignación de ticket {TicketId} a usuario {UserId}", ticketId, userId);

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                LogWarning("Ticket {TicketId} no encontrado para asignación", ticketId);
                throw new KeyNotFoundException("Ticket no encontrado.");
            }

            if (ticket.Status == Status.Closed)
            {
                LogWarning("Intento de asignar ticket cerrado {TicketId}", ticketId);
                throw new InvalidOperationException("No se puede asignar un ticket cerrado.");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                LogWarning("Usuario {UserId} no encontrado para asignación de ticket {TicketId}", userId, ticketId);
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            if (user.Role != "Agent")
            {
                LogWarning("Usuario {UserId} no es Agent, no puede ser asignado tickets", userId);
                throw new UnauthorizedAccessException("Solo los agentes pueden ser asignados a tickets.");
            }

            ticket.AssignedToId = userId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            // Crear historial de cambio
            var history = new TicketHistory
            {
                TicketId = ticketId,
                ChangedById = userId, // Asumir que el asignador es el usuario actual, ajustar si necesario
                OldAssignedToId = ticket.AssignedToId,
                NewAssignedToId = userId,
                ChangeDescription = $"Ticket asignado a {user.FullName}"
            };
            _context.TicketHistories.Add(history);
            await _unitOfWork.SaveChangesAsync();

            LogInformation("Ticket {TicketId} asignado exitosamente a usuario {UserId}", ticketId, userId);

            return ticket;
        }

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
        /// else
        /// {
        ///     // Manejar error: result.Error
        /// }
        /// </example>
        public async Task<Result> UnassignTicketAsync(int ticketId, CancellationToken ct)
        {
            // Guard clauses
            if (ticketId <= 0)
                return Result.Failure("El identificador del ticket debe ser mayor a cero.");

            ct.ThrowIfCancellationRequested();

            try
            {
                LogInformation("Iniciando desasignación de ticket {TicketId}", ticketId);

                var ticket = await _ticketRepository.GetByIdAsync(ticketId);
                if (ticket == null)
                {
                    LogWarning("Ticket {TicketId} no encontrado para desasignación", ticketId);
                    return Result.Failure("Ticket no encontrado.");
                }

                if (ticket.AssignedToId == null)
                {
                    LogWarning("Ticket {TicketId} ya está desasignado", ticketId);
                    return Result.Failure("El ticket ya está desasignado.");
                }

                var oldAssignedToId = ticket.AssignedToId;
                ticket.AssignedToId = null;
                ticket.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();

                // Crear historial de cambio
                var history = new TicketHistory
                {
                    TicketId = ticketId,
                    ChangedById = 1, // Asumir usuario sistema, ajustar si necesario
                    OldAssignedToId = oldAssignedToId,
                    NewAssignedToId = null,
                    ChangeDescription = "Ticket desasignado"
                };
                _context.TicketHistories.Add(history);
                await _unitOfWork.SaveChangesAsync();

                LogInformation("Ticket {TicketId} desasignado exitosamente", ticketId);

                return Result.Success();
            }
            catch (Exception ex)
            {
                LogError(ex, "Error al desasignar ticket {TicketId}", ticketId);
                return Result.Failure("Error interno al desasignar el ticket.");
            }
        }

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
        public async Task<Result<List<Ticket>>> GetTicketsByUserAsync(int userId, CancellationToken ct)
        {
            // Guard clauses
            if (userId <= 0)
                return Result<List<Ticket>>.Failure("El identificador del usuario debe ser mayor a cero.");

            ct.ThrowIfCancellationRequested();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                LogInformation("Obteniendo tickets asignados al usuario {UserId}", userId);

                var tickets = await _context.Tickets
                    .AsNoTracking()
                    .Where(t => t.AssignedToId == userId)
                    .Include(t => t.AssignedTo)
                    .Include(t => t.CreatedBy)
                    .Include(t => t.Comments)
                    .Include(t => t.History)
                    .ToListAsync(ct);

                stopwatch.Stop();
                LogInformation("Encontrados {Count} tickets asignados al usuario {UserId} en {ElapsedMs} ms", tickets.Count, userId, stopwatch.ElapsedMilliseconds);

                return Result<List<Ticket>>.Success(tickets);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogError(ex, "Error al obtener tickets para usuario {UserId} después de {ElapsedMs} ms", userId, stopwatch.ElapsedMilliseconds);
                return Result<List<Ticket>>.Failure("Error interno al obtener los tickets.");
            }
        }
    }
}