using AutoMapper;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Mappings;

/// <summary>
/// AutoMapper profile for role-related mappings
/// </summary>
public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        // Role to RoleDto
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // CreateRoleDto to Role
        CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        // UpdateRoleDto to Role
        CreateMap<UpdateRoleDto, Role>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}