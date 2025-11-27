using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for ticket business logic operations - orchestrates business rules, mapping, and persistence
    /// </summary>
    public class TicketService : BaseService, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketBusinessRules _businessRules;
        private readonly ITicketMapper _mapper;
        private readonly ApplicationDbContext _context;

        public TicketService(
            ITicketRepository ticketRepository,
            IUnitOfWork unitOfWork,
            ITicketBusinessRules businessRules,
            ITicketMapper mapper,
            ApplicationDbContext context,
            ILogger<TicketService> logger) : base(logger)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _mapper = mapper;
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Result<PagedResponse<TicketDto>>> GetTicketsAsync(GetTicketsQueryParameters parameters)
        {
            try
            {
                var pagedTickets = await _ticketRepository.GetAllAsync(parameters, CancellationToken.None);

                // Map to DTOs using dedicated mapper
                var ticketDtos = _mapper.MapToDtos(pagedTickets.Items);

                var result = new PagedResponse<TicketDto>
                {
                    Items = ticketDtos.ToList(),
                    TotalItems = pagedTickets.TotalItems,
                    Page = pagedTickets.Page,
                    PageSize = pagedTickets.PageSize
                };

                return Result<PagedResponse<TicketDto>>.Success(result);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error retrieving tickets");
                return Result<PagedResponse<TicketDto>>.Failure(ex.Message, "DatabaseError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<Ticket>> CreateAsync(CreateTicketDto dto, int createdByUserId, CancellationToken ct)
        {
            try
            {
                // Guard clauses
                if (dto == null) return Result<Ticket>.Failure("Ticket data cannot be null", "ValidationError");
                if (createdByUserId <= 0) return Result<Ticket>.Failure("Invalid user ID", "ValidationError");

                // Validate business rules
                await _businessRules.ValidateTicketCreationAsync(dto, createdByUserId, ct);

                // Map to entity
                var ticket = _mapper.MapToEntity(dto, createdByUserId);

                // Save to repository
                var createdTicket = await _ticketRepository.AddAsync(ticket, ct);

                // Create history record
                await _businessRules.CreateTicketHistoryAsync(createdTicket, "Ticket created", createdByUserId, ct);

                LogOperationSuccess(createdTicket.Id, createdByUserId, "CreateTicket");

                return Result<Ticket>.Success(createdTicket);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error creating ticket for user {UserId}", createdByUserId);
                return Result<Ticket>.Failure(ex.Message, "CreationError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<Ticket>> UpdateAsync(int id, UpdateTicketDto dto, CancellationToken ct)
        {
            try
            {
                // Guard clauses
                if (id <= 0) return Result<Ticket>.Failure("Invalid ticket ID", "ValidationError");
                if (dto == null) return Result<Ticket>.Failure("Update data cannot be null", "ValidationError");

                // Validate business rules
                await _businessRules.ValidateTicketUpdateAsync(id, dto, ct);

                // Get existing ticket
                var ticket = await _ticketRepository.GetByIdAsync(id, false, ct);
                if (ticket == null)
                {
                    return Result<Ticket>.Failure($"Ticket with ID {id} not found", "NotFound");
                }

                // Store old values for history
                var oldStatus = ticket.Status;
                var oldPriority = ticket.Priority;
                var oldAssignedToId = ticket.AssignedToId;

                // Apply update
                _mapper.ApplyUpdate(ticket, dto);

                // Save changes
                await _ticketRepository.UpdateAsync(ticket, ct);

                // Create history record if there were changes
                if (oldStatus != ticket.Status || oldPriority != ticket.Priority || oldAssignedToId != ticket.AssignedToId)
                {
                    await _businessRules.CreateTicketHistoryAsync(ticket, "Ticket updated", 0, ct); // TODO: Pass actual user ID
                }

                LogInformation("Ticket {TicketId} updated", id);

                return Result<Ticket>.Success(ticket);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error updating ticket {TicketId}", id);
                return Result<Ticket>.Failure(ex.Message, "UpdateError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<TicketDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id, true, ct);
                if (ticket == null)
                {
                    return Result<TicketDto>.Failure($"Ticket with ID {id} not found", "NotFound");
                }
                var dto = _mapper.MapToDto(ticket);
                return Result<TicketDto>.Success(dto);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error retrieving ticket {TicketId}", id);
                return Result<TicketDto>.Failure(ex.Message, "RetrievalError");
            }
        }

        /// <inheritdoc />
        public async Task<Result> DeleteAsync(int id, CancellationToken ct)
        {
            try
            {
                // Guard clauses
                if (id <= 0) return Result.Failure("Invalid ticket ID", "ValidationError");

                var ticket = await _ticketRepository.GetByIdAsync(id, false, ct);
                if (ticket == null)
                {
                    return Result.Failure($"Ticket with ID {id} not found", "NotFound");
                }

                // Soft delete
                ticket.IsDeleted = true;
                ticket.UpdatedAt = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket, ct);

                LogOperationSuccess(ticket.Id, null, "DeleteTicket");
                return Result.Success();
            }
            catch (Exception ex)
            {
                LogError(ex, "Error deleting ticket {TicketId}", id);
                return Result.Failure(ex.Message, "DeletionError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<List<TicketDto>>> GetUserTicketsAsync(int userId, CancellationToken ct)
        {
            try
            {
                // Get tickets created by or assigned to the user
                var tickets = await _context.Tickets
                    .AsNoTracking()
                    .Where(t => !t.IsDeleted && (t.CreatedById == userId || t.AssignedToId == userId))
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync(ct);

                var dtos = _mapper.MapToDtos(tickets).ToList();
                return Result<List<TicketDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error retrieving tickets for user {UserId}", userId);
                return Result<List<TicketDto>>.Failure(ex.Message, "RetrievalError");
            }
        }

        /// <inheritdoc />
        [Obsolete("Use GetTicketHistoryAsync with filter parameter instead")]
        public async Task<Result<List<TicketHistory>>> GetTicketHistoryAsync(int ticketId, CancellationToken ct)
        {
            try
            {
                var history = await _context.TicketHistories
                    .AsNoTracking()
                    .Where(h => h.TicketId == ticketId)
                    .Include(h => h.ChangedBy)
                    .OrderByDescending(h => h.ChangedAt)
                    .ToListAsync(ct);

                return Result<List<TicketHistory>>.Success(history);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error retrieving history for ticket {TicketId}", ticketId);
                return Result<List<TicketHistory>>.Failure(ex.Message, "RetrievalError");
            }
        }

        /// <inheritdoc />
        public async Task<Result<PagedResponse<TicketHistoryDto>>> GetTicketHistoryAsync(
            int ticketId, 
            TicketHistoryFilterDto? filter, 
            CancellationToken ct)
        {
            try
            {
                filter ??= new TicketHistoryFilterDto();

                // Build query with filters
                var query = _context.TicketHistories
                    .AsNoTracking()
                    .Where(h => h.TicketId == ticketId)
                    .Include(h => h.ChangedBy)
                    .AsQueryable();

                // Apply date filters
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(h => h.ChangedAt >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(h => h.ChangedAt <= filter.ToDate.Value);
                }

                // Apply user filter
                if (filter.ChangedById.HasValue)
                {
                    query = query.Where(h => h.ChangedById == filter.ChangedById.Value);
                }

                // Get total count for pagination
                var totalCount = await query.CountAsync(ct);

                // Apply pagination and ordering
                var historyEntries = await query
                    .OrderByDescending(h => h.ChangedAt)
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync(ct);

                // Get unique user IDs for assignment names
                var assignedUserIds = historyEntries
                    .SelectMany(h => new[] { h.OldAssignedToId, h.NewAssignedToId })
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .Distinct()
                    .ToList();

                // Fetch user names in a single query
                var userNames = await _context.Users
                    .AsNoTracking()
                    .Where(u => assignedUserIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => u.FullName, ct);

                // Map to DTOs with enriched data
                var dtos = historyEntries.Select(h => MapToHistoryDto(h, userNames)).ToList();

                var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);
                var response = new PagedResponse<TicketHistoryDto>
                {
                    Items = dtos,
                    TotalItems = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    TotalPages = totalPages,
                    HasNextPage = filter.Page < totalPages,
                    HasPreviousPage = filter.Page > 1
                };

                LogInformation("Retrieved {Count} history entries for ticket {TicketId} (page {Page})", 
                    dtos.Count, ticketId, filter.Page);

                return Result<PagedResponse<TicketHistoryDto>>.Success(response);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error retrieving history for ticket {TicketId}", ticketId);
                return Result<PagedResponse<TicketHistoryDto>>.Failure(ex.Message, "RetrievalError");
            }
        }

        /// <summary>
        /// Maps a TicketHistory entity to TicketHistoryDto with enriched user names and calculated changes
        /// </summary>
        private static TicketHistoryDto MapToHistoryDto(TicketHistory history, Dictionary<int, string> userNames)
        {
            var changes = new List<TicketHistoryChangeDto>();

            // Detect status change
            if (history.OldStatus != history.NewStatus || !history.OldStatus.HasValue)
            {
                changes.Add(new TicketHistoryChangeDto
                {
                    Field = "Status",
                    OldValue = history.OldStatus?.ToString(),
                    NewValue = history.NewStatus.ToString(),
                    OldDisplayValue = history.OldStatus?.ToString() ?? "N/A",
                    NewDisplayValue = history.NewStatus.ToString()
                });
            }

            // Detect priority change
            if (history.OldPriority != history.NewPriority || !history.OldPriority.HasValue)
            {
                changes.Add(new TicketHistoryChangeDto
                {
                    Field = "Priority",
                    OldValue = history.OldPriority?.ToString(),
                    NewValue = history.NewPriority.ToString(),
                    OldDisplayValue = history.OldPriority?.ToString() ?? "N/A",
                    NewDisplayValue = history.NewPriority.ToString()
                });
            }

            // Detect assignment change
            if (history.OldAssignedToId != history.NewAssignedToId)
            {
                var oldName = history.OldAssignedToId.HasValue && userNames.ContainsKey(history.OldAssignedToId.Value)
                    ? userNames[history.OldAssignedToId.Value]
                    : "Sin asignar";
                var newName = history.NewAssignedToId.HasValue && userNames.ContainsKey(history.NewAssignedToId.Value)
                    ? userNames[history.NewAssignedToId.Value]
                    : "Sin asignar";

                changes.Add(new TicketHistoryChangeDto
                {
                    Field = "AssignedTo",
                    OldValue = history.OldAssignedToId?.ToString(),
                    NewValue = history.NewAssignedToId?.ToString(),
                    OldDisplayValue = oldName,
                    NewDisplayValue = newName
                });
            }

            return new TicketHistoryDto
            {
                Id = history.Id,
                TicketId = history.TicketId,
                ChangedById = history.ChangedById,
                ChangedByName = history.ChangedBy?.FullName ?? "Usuario desconocido",
                ChangedByEmail = history.ChangedBy?.Email,
                ChangedAt = history.ChangedAt,
                OldStatus = history.OldStatus?.ToString(),
                NewStatus = history.NewStatus.ToString(),
                OldPriority = history.OldPriority?.ToString(),
                NewPriority = history.NewPriority.ToString(),
                OldAssignedToId = history.OldAssignedToId,
                OldAssignedToName = history.OldAssignedToId.HasValue && userNames.ContainsKey(history.OldAssignedToId.Value)
                    ? userNames[history.OldAssignedToId.Value]
                    : null,
                NewAssignedToId = history.NewAssignedToId,
                NewAssignedToName = history.NewAssignedToId.HasValue && userNames.ContainsKey(history.NewAssignedToId.Value)
                    ? userNames[history.NewAssignedToId.Value]
                    : null,
                ChangeDescription = history.ChangeDescription,
                IsCreation = !history.OldStatus.HasValue && !history.OldPriority.HasValue && !history.OldAssignedToId.HasValue,
                Changes = changes
            };
        }
    }
}