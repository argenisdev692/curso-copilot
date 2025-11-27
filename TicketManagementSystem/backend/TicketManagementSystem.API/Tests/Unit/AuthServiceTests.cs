using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;
using Xunit;

namespace TicketManagementSystem.API.Tests.Unit;

/// <summary>
/// Unit tests for AuthService
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<AuthService>>();

        // Setup configuration
        _configurationMock.Setup(c => c["Jwt:ExpiryInMinutes"]).Returns("60");

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _jwtTokenServiceMock.Object,
            _configurationMock.Object,
            _loggerMock.Object);
    }

    /// <summary>
    /// Test successful login
    /// </summary>
    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var user = new User
        {
            Id = 1,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = "Test User",
            Role = "User",
            IsActive = true
        };
        var token = "jwt-token";
        var refreshToken = "refresh-token";

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
        _jwtTokenServiceMock.Setup(j => j.GenerateJwtToken(user)).Returns(token);
        _jwtTokenServiceMock.Setup(j => j.GenerateRefreshToken()).Returns(refreshToken);

        // Act
        var result = await _authService.LoginAsync(email, password);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(token);
        result.RefreshToken.Should().Be(refreshToken);
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(email);
        result.User.FullName.Should().Be(user.FullName);
        result.User.Role.Should().Be(user.Role);
    }

    /// <summary>
    /// Test login with non-existent user
    /// </summary>
    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "password123";

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User)null!);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials");
    }

    /// <summary>
    /// Test login with inactive user
    /// </summary>
    [Fact]
    public async Task LoginAsync_InactiveUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var email = "inactive@example.com";
        var password = "password123";
        var user = new User
        {
            Id = 1,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = "Inactive User",
            Role = "User",
            IsActive = false
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials");
    }

    /// <summary>
    /// Test login with wrong password
    /// </summary>
    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "wrongpassword";
        var user = new User
        {
            Id = 1,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            FullName = "Test User",
            Role = "User",
            IsActive = true
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials");
    }

    /// <summary>
    /// Test successful registration
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ValidData_ReturnsRegisterResponse()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "newuser@example.com",
            Password = "Password123!",
            FullName = "New User",
            Role = "User"
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email)).ReturnsAsync((User)null!);
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("User registered successfully");
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(registerDto.Email);
        result.User.FullName.Should().Be(registerDto.FullName);
        result.User.Role.Should().Be(registerDto.Role);

        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == registerDto.Email &&
            u.FullName == registerDto.FullName &&
            u.Role == registerDto.Role &&
            u.IsActive == true)), Times.Once);
    }

    /// <summary>
    /// Test registration with existing email
    /// </summary>
    [Fact]
    public async Task RegisterAsync_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "Password123!",
            FullName = "Existing User",
            Role = "User"
        };

        var existingUser = new User { Id = 1, Email = registerDto.Email };
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email)).ReturnsAsync(existingUser);

        // Act
        Func<Task> act = async () => await _authService.RegisterAsync(registerDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User with this email already exists");
    }

    /// <summary>
    /// Test registration with invalid role
    /// </summary>
    [Fact]
    public async Task RegisterAsync_InvalidRole_ThrowsInvalidOperationException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            FullName = "Test User",
            Role = "InvalidRole"
        };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(registerDto.Email)).ReturnsAsync((User)null!);

        // Act
        Func<Task> act = async () => await _authService.RegisterAsync(registerDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid role specified");
    }

    /// <summary>
    /// Test token refresh
    /// </summary>
    [Fact]
    public async Task RefreshTokenAsync_ValidToken_ReturnsRefreshResponse()
    {
        // Arrange
        var refreshToken = "valid-refresh-token";

        // Act
        var result = await _authService.RefreshTokenAsync(refreshToken);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    /// <summary>
    /// Test logout
    /// </summary>
    [Fact]
    public async Task LogoutAsync_ValidToken_CompletesSuccessfully()
    {
        // Arrange
        var refreshToken = "refresh-token";

        // Act
        await _authService.LogoutAsync(refreshToken);

        // Assert
        // Method should complete without throwing
    }

    /// <summary>
    /// Test login with null email
    /// </summary>
    [Fact]
    public async Task LoginAsync_NullEmail_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _authService.LoginAsync(null!, "password");

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Test login with null password
    /// </summary>
    [Fact]
    public async Task LoginAsync_NullPassword_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _authService.LoginAsync("email@example.com", null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Test register with null dto
    /// </summary>
    [Fact]
    public async Task RegisterAsync_NullDto_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _authService.RegisterAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    /// <summary>
    /// Test refresh token with null token
    /// </summary>
    [Fact]
    public async Task RefreshTokenAsync_NullToken_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _authService.RefreshTokenAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}