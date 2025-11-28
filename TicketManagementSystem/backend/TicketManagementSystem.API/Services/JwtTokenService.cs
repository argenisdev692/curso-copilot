using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services;

/// <summary>
/// Interface for JWT token operations
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the user
    /// </summary>
    string GenerateJwtToken(User user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates and gets user from refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token to validate</param>
    /// <returns>User if token is valid, null otherwise</returns>
    Task<User?> ValidateRefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Stores a refresh token for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="tokenHash">Hashed token</param>
    /// <param name="expiresAt">Expiration date</param>
    /// <param name="createdByIp">IP address that created the token</param>
    /// <returns>The stored refresh token</returns>
    Task<RefreshToken> StoreRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAt, string? createdByIp = null);

    /// <summary>
    /// Revokes a refresh token
    /// </summary>
    /// <param name="tokenHash">Token hash to revoke</param>
    /// <param name="reason">Reason for revocation</param>
    Task RevokeRefreshTokenAsync(string tokenHash, string? reason = null);

    /// <summary>
    /// Revokes all refresh tokens for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="reason">Reason for revocation</param>
    Task RevokeAllRefreshTokensForUserAsync(int userId, string? reason = null);
}

/// <summary>
/// Implementation of JWT token service
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public JwtTokenService(
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository)
    {
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <inheritdoc />
    public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
    {
        // Hash the incoming token for comparison
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);
        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        // Update last used timestamp
        storedToken.LastUsedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(storedToken);

        return storedToken.User;
    }

    /// <inheritdoc />
    public async Task<RefreshToken> StoreRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAt, string? createdByIp = null)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = createdByIp,
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        return refreshToken;
    }

    /// <inheritdoc />
    public async Task RevokeRefreshTokenAsync(string tokenHash, string? reason = null)
    {
        await _refreshTokenRepository.RevokeTokenAsync(tokenHash, reason);
    }

    /// <inheritdoc />
    public async Task RevokeAllRefreshTokensForUserAsync(int userId, string? reason = null)
    {
        await _refreshTokenRepository.RevokeAllTokensForUserAsync(userId, reason);
    }

    /// <summary>
    /// Hashes a token using SHA256 for secure storage
    /// </summary>
    private static string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hashBytes);
    }
}