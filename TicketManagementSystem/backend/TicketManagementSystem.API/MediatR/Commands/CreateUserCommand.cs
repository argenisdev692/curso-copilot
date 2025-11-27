using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.MediatR.Commands;

/// <summary>
/// Command to create a new user
/// </summary>
public class CreateUserCommand : IRequest<UserDto>
{
    public CreateUserDto UserDto { get; set; } = null!;
}

/// <summary>
/// Handler for CreateUserCommand
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.CreateUserAsync(request.UserDto);
    }
}