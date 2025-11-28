using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            // Store the refresh token securely
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpiryInDays"] ?? "7"));
            var tokenHash = HashToken(refreshTokenValue);
            await _jwtTokenService.StoreRefreshTokenAsync(user.Id, tokenHash, refreshTokenExpiry);

            LogInformation("User {Email} logged in successfully", email);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshTokenValue,
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
            LogInformation("Attempting token refresh");

            var user = await _jwtTokenService.ValidateRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                LogWarning("Invalid or expired refresh token");
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            // Generate new tokens
            var newJwtToken = _jwtTokenService.GenerateJwtToken(user);
            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            // Revoke old refresh token
            var oldTokenHash = HashToken(refreshToken);
            await _jwtTokenService.RevokeRefreshTokenAsync(oldTokenHash, "Token refresh");

            // Store new refresh token
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpiryInDays"] ?? "7"));
            var newTokenHash = HashToken(newRefreshTokenValue);
            await _jwtTokenService.StoreRefreshTokenAsync(user.Id, newTokenHash, refreshTokenExpiry);

            LogInformation("Token refreshed successfully for user {Email}", user.Email);

            return new RefreshTokenResponseDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60"))
            };
        }

        /// <inheritdoc />
        public async Task LogoutAsync(string refreshToken)
        {
            var tokenPreview = !string.IsNullOrEmpty(refreshToken) && refreshToken.Length >= 10
                ? refreshToken.Substring(0, 10)
                : refreshToken ?? "null";

            LogInformation("Logout requested for refresh token: {RefreshToken}", tokenPreview);

            // Revoke the refresh token
            var tokenHash = HashToken(refreshToken);
            await _jwtTokenService.RevokeRefreshTokenAsync(tokenHash, "User logout");

            LogInformation("Refresh token invalidated successfully");

            await Task.CompletedTask;
        }

        /// <summary>
        /// Hashes a token using SHA256 for secure storage
        /// </summary>
        private static string HashToken(string token)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashBytes);
        }
    }
}