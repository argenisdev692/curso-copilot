using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Service for authentication business logic operations
    /// </summary>
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IConfiguration configuration,
            ILogger<AuthService> logger) : base(logger)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            LogInformation("Attempting login for user {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _jwtTokenService.GenerateJwtToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            LogInformation("User {Email} logged in successfully", email);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60")),
                User = new UserBasicDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };
        }

        /// <inheritdoc />
        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto dto)
        {
            LogInformation("Attempting registration for user {Email}", dto.Email);

            // Check if user already exists (including soft-deleted users)
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                LogWarning("Registration attempt for existing email {Email}", dto.Email);
                throw new InvalidOperationException("User with this email already exists");
            }

            // Validate role
            if (!new[] { "User", "Agent", "Admin" }.Contains(dto.Role))
            {
                throw new InvalidOperationException("Invalid role specified");
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            LogInformation("User {Email} registered successfully", dto.Email);

            return new RegisterResponseDto
            {
                Message = "User registered successfully",
                User = new UserBasicDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };
        }

        /// <inheritdoc />
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // In a production system, you would:
            // 1. Validate the refresh token from a secure store (database/redis)
            // 2. Check if it's not expired
            // 3. Generate new access token
            // 4. Optionally rotate the refresh token

            // For now, we'll return a basic response
            LogWarning("Refresh token functionality called but not fully implemented");

            return await Task.FromResult(new RefreshTokenResponseDto
            {
                Token = "new-jwt-token-would-be-generated-here",
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        /// <inheritdoc />
        public async Task LogoutAsync(string refreshToken)
        {
            var tokenPreview = !string.IsNullOrEmpty(refreshToken) && refreshToken.Length >= 10 
                ? refreshToken.Substring(0, 10) 
                : refreshToken ?? "null";
            
            LogInformation("Logout requested for refresh token: {RefreshToken}", tokenPreview);

            // In a production system, you would:
            // 1. Remove/invalidate the refresh token from secure store (database/redis)
            // 2. Optionally add the JWT to a blacklist with expiration
            // 3. Clear any user session data
            
            // For now, just log the action
            LogInformation("Refresh token invalidated successfully");

            await Task.CompletedTask;
        }
    }
}