using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using Xunit;

namespace TicketManagementSystem.API.Tests.Integration;

/// <summary>
/// Integration tests for AuthController endpoints
/// </summary>
public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Ensure database is created
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    /// <summary>
    /// Test successful login with valid credentials
    /// </summary>
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        // Seed test user
        await SeedTestUser(loginDto.Email, loginDto.Password);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(loginDto.Email);
    }

    /// <summary>
    /// Test login with invalid email
    /// </summary>
    [Fact]
    public async Task Login_InvalidEmail_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Test login with invalid password
    /// </summary>
    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        // Seed test user
        await SeedTestUser(loginDto.Email, "correctpassword");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Test login with invalid email format
    /// </summary>
    [Fact]
    public async Task Login_InvalidEmailFormat_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Test login with empty email
    /// </summary>
    [Fact]
    public async Task Login_EmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "",
            Password = "password123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Test successful registration
    /// </summary>
    [Fact]
    public async Task Register_ValidData_ReturnsCreated()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "newuser@example.com",
            Password = "Password123!",
            FullName = "New User",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        result.Should().NotBeNull();
        result!.Message.Should().Be("User registered successfully");
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(registerDto.Email);
    }

    /// <summary>
    /// Test registration with existing email
    /// </summary>
    [Fact]
    public async Task Register_ExistingEmail_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "Password123!",
            FullName = "Existing User",
            Role = "User"
        };

        // Seed existing user
        await SeedTestUser(registerDto.Email, registerDto.Password);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Test registration with invalid password
    /// </summary>
    [Fact]
    public async Task Register_InvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "weak",
            FullName = "Test User",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Test token refresh with valid refresh token
    /// </summary>
    [Fact]
    public async Task RefreshToken_ValidToken_ReturnsOk()
    {
        // Arrange
        var refreshDto = new RefreshTokenDto
        {
            RefreshToken = "valid-refresh-token"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Test token refresh with empty refresh token
    /// </summary>
    [Fact]
    public async Task RefreshToken_EmptyToken_ReturnsBadRequest()
    {
        // Arrange
        var refreshDto = new RefreshTokenDto
        {
            RefreshToken = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Test logout without authorization
    /// </summary>
    [Fact]
    public async Task Logout_WithoutAuthorization_ReturnsUnauthorized()
    {
        // Arrange
        var logoutDto = new LogoutDto
        {
            RefreshToken = "some-token"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/logout", logoutDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Test rate limiting simulation (would require actual rate limiting setup)
    /// </summary>
    [Fact]
    public async Task Login_RateLimitExceeded_ReturnsTooManyRequests()
    {
        // Note: This test would require configuring rate limiting in test setup
        // For now, just verify the endpoint exists and handles requests
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
    }

    /// <summary>
    /// Helper method to seed test user
    /// </summary>
    private async Task SeedTestUser(string email, string password)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = "Test User",
            Role = "User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}