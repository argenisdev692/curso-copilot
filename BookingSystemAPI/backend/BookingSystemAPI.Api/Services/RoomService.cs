using AutoMapper;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Rooms;
using BookingSystemAPI.Api.Models;
using BookingSystemAPI.Api.Repositories;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Implementación del servicio de salas.
/// </summary>
public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RoomService> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de salas.
    /// </summary>
    public RoomService(
        IRoomRepository roomRepository,
        IMapper mapper,
        ILogger<RoomService> logger)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<RoomDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _roomRepository.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);
        return Result<IEnumerable<RoomDto>>.Success(dtos);
    }

    /// <inheritdoc />
    public async Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Result<RoomDto>.Failure(new Error("NotFound", $"Sala con ID {id} no encontrada."));
        }

        return Result<RoomDto>.Success(_mapper.Map<RoomDto>(room));
    }

    /// <inheritdoc />
    public async Task<Result<RoomDto>> CreateAsync(CreateRoomDto dto, CancellationToken cancellationToken = default)
    {
        // Verificar nombre único
        if (await _roomRepository.ExistsByNameAsync(dto.Name, cancellationToken: cancellationToken))
        {
            return Result<RoomDto>.Failure(new Error("Conflict", $"Ya existe una sala con el nombre '{dto.Name}'."));
        }

        var room = _mapper.Map<Room>(dto);
        room.Status = RoomStatus.Available;

        await _roomRepository.AddAsync(room, cancellationToken);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sala creada: {RoomId} - {RoomName}", room.Id, room.Name);

        return Result<RoomDto>.Success(_mapper.Map<RoomDto>(room));
    }

    /// <inheritdoc />
    public async Task<Result<RoomDto>> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Result<RoomDto>.Failure(new Error("NotFound", $"Sala con ID {id} no encontrada."));
        }

        // Verificar nombre único (excluyendo la sala actual)
        if (await _roomRepository.ExistsByNameAsync(dto.Name, id, cancellationToken))
        {
            return Result<RoomDto>.Failure(new Error("Conflict", $"Ya existe otra sala con el nombre '{dto.Name}'."));
        }

        // Actualizar propiedades
        room.Name = dto.Name;
        room.Capacity = dto.Capacity;
        room.Equipment = dto.Equipment;
        room.Location = dto.Location;

        if (Enum.TryParse<RoomStatus>(dto.Status, true, out var status))
        {
            room.Status = status;
        }

        _roomRepository.Update(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sala actualizada: {RoomId}", id);

        return Result<RoomDto>.Success(_mapper.Map<RoomDto>(room));
    }

    /// <inheritdoc />
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Result.Failure(new Error("NotFound", $"Sala con ID {id} no encontrada."));
        }

        _roomRepository.Delete(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sala eliminada: {RoomId}", id);

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> SetMaintenanceAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Result.Failure(new Error("NotFound", $"Sala con ID {id} no encontrada."));
        }

        room.Status = RoomStatus.Maintenance;
        _roomRepository.Update(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sala {RoomId} puesta en mantenimiento", id);

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> SetAvailableAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            return Result.Failure(new Error("NotFound", $"Sala con ID {id} no encontrada."));
        }

        room.Status = RoomStatus.Available;
        _roomRepository.Update(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Sala {RoomId} disponible", id);

        return Result.Success();
    }
}
