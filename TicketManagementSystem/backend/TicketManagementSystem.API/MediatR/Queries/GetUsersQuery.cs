using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.MediatR.Queries;

/// <summary>
/// Query to get paginated users
/// </summary>
public class GetUsersQuery : IRequest<PagedResponse<UserDto>>
{
    public GetUsersQueryParameters Parameters { get; set; } = new();
}

/// <summary>
/// Handler for GetUsersQuery
/// </summary>
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResponse<UserDto>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<PagedResponse<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetUsersAsync(request.Parameters);
    }
}