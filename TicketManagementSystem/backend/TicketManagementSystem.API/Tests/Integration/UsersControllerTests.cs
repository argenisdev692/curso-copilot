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
/// Integration tests for UsersController endpoints
/// </summary>
public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
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

    #region GET /api/users

    [Fact]
    public async Task GetUsers_ValidRequest_ReturnsOkWithUsers()
    {
        // Arrange
        var queryParams = new GetUsersQueryParameters
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<UserDto>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetUsers_InvalidPage_ReturnsBadRequest(int invalidPage)
    {
        // Arrange
        var queryParams = $"?page={invalidPage}&pageSize=10";

        // Act
        var response = await _client.GetAsync($"/api/users{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task GetUsers_InvalidPageSize_ReturnsBadRequest(int invalidPageSize)
    {
        // Arrange
        var queryParams = $"?page=1&pageSize={invalidPageSize}";

        // Act
        var response = await _client.GetAsync($"/api/users{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUsers_WithRoleFilter_ReturnsFilteredResults()
    {
        // Arrange
        var queryParams = "?role=Admin";

        // Act
        var response = await _client.GetAsync($"/api/users{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<UserDto>>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUsers_WithSearchFilter_ReturnsFilteredResults()
    {
        // Arrange
        var queryParams = "?search=test";

        // Act
        var response = await _client.GetAsync($"/api/users{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<UserDto>>();
        result.Should().NotBeNull();
    }

    #endregion

    #region GET /api/users/{id}

    [Fact]
    public async Task GetUserById_ExistingUser_ReturnsOk()
    {
        // Arrange - First create a user
        var createRequest = new CreateUserDto
        {
            Email = "testuser@example.com",
            FullName = "Test User",
            Password = "TestPassword123!",
            Role = "User"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        // Act
        var response = await _client.GetAsync($"/api/users/{createdUser!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdUser.Id);
        result.Email.Should().Be(createRequest.Email);
        result.FullName.Should().Be(createRequest.FullName);
    }

    [Fact]
    public async Task GetUserById_NonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.GetAsync($"/api/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetUserById_InvalidId_ReturnsBadRequest(int invalidId)
    {
        // Act
        var response = await _client.GetAsync($"/api/users/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region POST /api/users

    [Fact]
    public async Task CreateUser_ValidData_ReturnsCreated()
    {
        // Arrange
        var request = new CreateUserDto
        {
            Email = "newuser@example.com",
            FullName = "New User",
            Password = "SecurePassword123!",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Email.Should().Be(request.Email);
        result.FullName.Should().Be(request.FullName);
        result.Role.Should().Be(request.Role);
        result.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateUser_InvalidEmail_ReturnsBadRequest(string? invalidEmail)
    {
        // Arrange
        var request = new CreateUserDto
        {
            Email = invalidEmail!,
            FullName = "Valid Name",
            Password = "ValidPassword123!",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateUser_InvalidFullName_ReturnsBadRequest(string? invalidFullName)
    {
        // Arrange
        var request = new CreateUserDto
        {
            Email = "valid@example.com",
            FullName = invalidFullName!,
            Password = "ValidPassword123!",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("weak")]
    [InlineData("12345678")]
    public async Task CreateUser_InvalidPassword_ReturnsBadRequest(string? invalidPassword)
    {
        // Arrange
        var request = new CreateUserDto
        {
            Email = "valid@example.com",
            FullName = "Valid Name",
            Password = invalidPassword!,
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("InvalidRole")]
    public async Task CreateUser_InvalidRole_ReturnsBadRequest(string? invalidRole)
    {
        // Arrange
        var request = new CreateUserDto
        {
            Email = "valid@example.com",
            FullName = "Valid Name",
            Password = "ValidPassword123!",
            Role = invalidRole!
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_DuplicateEmail_ReturnsConflict()
    {
        // Arrange - Create first user
        var firstRequest = new CreateUserDto
        {
            Email = "duplicate@example.com",
            FullName = "First User",
            Password = "Password123!",
            Role = "User"
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/users", firstRequest);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Try to create second user with same email
        var secondRequest = new CreateUserDto
        {
            Email = "duplicate@example.com",
            FullName = "Second User",
            Password = "Password123!",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", secondRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    #endregion

    #region PUT /api/users/{id}

    [Fact]
    public async Task UpdateUser_ValidData_ReturnsOk()
    {
        // Arrange - Create a user first
        var createRequest = new CreateUserDto
        {
            Email = "updateuser@example.com",
            FullName = "Original Name",
            Password = "Password123!",
            Role = "User"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        var updateRequest = new UpdateUserDto
        {
            Email = "updateduser@example.com",
            FullName = "Updated Name",
            Role = "Admin",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{createdUser!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.Email.Should().Be(updateRequest.Email);
        result.FullName.Should().Be(updateRequest.FullName);
        result.Role.Should().Be(updateRequest.Role);
        result.IsActive.Should().Be(updateRequest.IsActive);
    }

    [Fact]
    public async Task UpdateUser_NonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;
        var updateRequest = new UpdateUserDto
        {
            Email = "test@example.com",
            FullName = "Test User",
            Role = "User",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{nonExistentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateUser_InvalidEmail_ReturnsBadRequest(string? invalidEmail)
    {
        // Arrange - Create a user first
        var createRequest = new CreateUserDto
        {
            Email = "testuser@example.com",
            FullName = "Test User",
            Password = "Password123!",
            Role = "User"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        var updateRequest = new UpdateUserDto
        {
            Email = invalidEmail!,
            FullName = "Valid Name",
            Role = "User",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{createdUser!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region DELETE /api/users/{id}

    [Fact]
    public async Task DeleteUser_ExistingUser_ReturnsNoContent()
    {
        // Arrange - Create a user first
        var createRequest = new CreateUserDto
        {
            Email = "deleteuser@example.com",
            FullName = "User to Delete",
            Password = "Password123!",
            Role = "User"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/users/{createdUser!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_NonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.DeleteAsync($"/api/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}