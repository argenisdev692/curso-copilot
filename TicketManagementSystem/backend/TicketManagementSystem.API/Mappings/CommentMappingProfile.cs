using AutoMapper;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Mappings
{
    /// <summary>
    /// AutoMapper profile for comment-related mappings
    /// </summary>
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            // Comment to CommentDto
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.FullName : string.Empty));

            // CreateCommentDto to Comment
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // UpdateCommentDto to Comment (for updates)
            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}