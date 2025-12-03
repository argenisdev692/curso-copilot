using AutoMapper;
using BookingSystemAPI.Api.DTOs.Auth;
using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.DTOs.Rooms;
using BookingSystemAPI.Api.Models;

namespace BookingSystemAPI.Api.Common.Mappings;

/// <summary>
/// Perfil de AutoMapper para configuración de mapeos de la aplicación.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Inicializa una nueva instancia del perfil de mapeo.
    /// </summary>
    public MappingProfile()
    {
        // Mapeos de Room
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateRoomDto, Room>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        // Mapeos de Booking
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room != null ? src.Room.Name : string.Empty));

        CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Room, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => BookingStatus.Confirmed))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<UpdateBookingDto, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RoomId, opt => opt.Ignore())
            .ForMember(dest => dest.Room, opt => opt.Ignore())
            .ForMember(dest => dest.OrganizerEmail, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        // Mapeos de User
        CreateMap<User, UserDto>();
    }
}
