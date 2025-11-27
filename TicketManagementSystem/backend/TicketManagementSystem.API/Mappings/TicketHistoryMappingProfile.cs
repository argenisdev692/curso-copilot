using AutoMapper;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Mappings
{
    /// <summary>
    /// AutoMapper profile para mapeos relacionados con el historial de tickets.
    /// Incluye lógica para enriquecer los DTOs con nombres de usuarios y detectar cambios.
    /// </summary>
    public class TicketHistoryMappingProfile : Profile
    {
        /// <summary>
        /// Inicializa los mapeos de TicketHistory a TicketHistoryDto
        /// </summary>
        public TicketHistoryMappingProfile()
        {
            CreateMap<TicketHistory, TicketHistoryDto>()
                .ForMember(dest => dest.ChangedByName, 
                    opt => opt.MapFrom(src => src.ChangedBy != null ? src.ChangedBy.FullName : "Usuario desconocido"))
                .ForMember(dest => dest.ChangedByEmail, 
                    opt => opt.MapFrom(src => src.ChangedBy != null ? src.ChangedBy.Email : null))
                .ForMember(dest => dest.OldStatus, 
                    opt => opt.MapFrom(src => src.OldStatus.HasValue ? src.OldStatus.Value.ToString() : null))
                .ForMember(dest => dest.NewStatus, 
                    opt => opt.MapFrom(src => src.NewStatus.ToString()))
                .ForMember(dest => dest.OldPriority, 
                    opt => opt.MapFrom(src => src.OldPriority.HasValue ? src.OldPriority.Value.ToString() : null))
                .ForMember(dest => dest.NewPriority, 
                    opt => opt.MapFrom(src => src.NewPriority.ToString()))
                .ForMember(dest => dest.IsCreation, 
                    opt => opt.MapFrom(src => !src.OldStatus.HasValue && !src.OldPriority.HasValue && !src.OldAssignedToId.HasValue))
                .ForMember(dest => dest.OldAssignedToName, 
                    opt => opt.Ignore()) // Se resuelve en el servicio
                .ForMember(dest => dest.NewAssignedToName, 
                    opt => opt.Ignore()) // Se resuelve en el servicio
                .ForMember(dest => dest.Changes, 
                    opt => opt.Ignore()); // Se calcula en el servicio
        }
    }
}
