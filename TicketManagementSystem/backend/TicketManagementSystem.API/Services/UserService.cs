using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for user business logic operations
    /// Follows Single Responsibility Principle by delegating password operations
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            ILogger<UserService> logger) : base(logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<PagedResponse<UserDto>> GetUsersAsync(GetUsersQueryParameters parameters)
        {
            // Delegate to repository for data access
            var usersResponse = await _userRepository.GetUsersAsync(parameters);

            // Map to DTOs
            var userDtos = _mapper.Map<List<UserDto>>(usersResponse.Items);

            return new PagedResponse<UserDto>
            {
                Items = userDtos,
                Page = usersResponse.Page,
                PageSize = usersResponse.PageSize,
                TotalItems = usersResponse.TotalItems,
                TotalPages = usersResponse.TotalPages
            };
        }

        /// <inheritdoc />
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            return _mapper.Map<UserDto>(user);
        }

        /// <inheritdoc />
        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            // Check if user already exists - business rule validation
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = _mapper.Map<User>(userDto);

            // Delegate password hashing to specialized service (SRP)
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);

            LogInformation("User {UserId} created", user.Id);

            return _mapper.Map<UserDto>(user);
        }

        /// <inheritdoc />
        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            _mapper.Map(userDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            LogInformation("User {UserId} updated", id);

            return _mapper.Map<UserDto>(user);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            // Soft delete - business logic
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            LogInformation("User {UserId} soft deleted", id);

            return true;
        }
    }
}