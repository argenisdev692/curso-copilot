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
/// Integration tests for TicketsController endpoints
/// </summary>
public class TicketsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TicketsControllerTests(WebApplicationFactory<Program> factory)
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

    #region GET /api/tickets

    [Fact]
    public async Task GetTickets_ValidRequest_ReturnsOkWithTickets()
    {
        // Arrange
        var queryParams = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var response = await _client.GetAsync("/api/tickets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<TicketResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetTickets_InvalidPage_ReturnsBadRequest(int invalidPage)
    {
        // Arrange
        var queryParams = $"?page={invalidPage}&pageSize=10";

        // Act
        var response = await _client.GetAsync($"/api/tickets{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task GetTickets_InvalidPageSize_ReturnsBadRequest(int invalidPageSize)
    {
        // Arrange
        var queryParams = $"?page=1&pageSize={invalidPageSize}";

        // Act
        var response = await _client.GetAsync($"/api/tickets{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTickets_WithStatusFilter_ReturnsFilteredResults()
    {
        // Arrange
        var queryParams = "?status=Open";

        // Act
        var response = await _client.GetAsync($"/api/tickets{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<TicketResponse>>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTickets_WithPriorityFilter_ReturnsFilteredResults()
    {
        // Arrange
        var queryParams = "?priority=High";

        // Act
        var response = await _client.GetAsync($"/api/tickets{queryParams}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<TicketResponse>>();
        result.Should().NotBeNull();
    }

    #endregion

    #region GET /api/tickets/{id}

    [Fact]
    public async Task GetTicketById_ExistingTicket_ReturnsOk()
    {
        // Arrange - Create a ticket first
        var createRequest = new CreateTicketRequest
        {
            Title = "Test Ticket",
            Description = "Test Description",
            Priority = "Medium"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Act
        var response = await _client.GetAsync($"/api/tickets/{createdTicket!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TicketResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(createdTicket.Id);
        result.Title.Should().Be(createRequest.Title);
    }

    [Fact]
    public async Task GetTicketById_NonExistentTicket_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.GetAsync($"/api/tickets/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetTicketById_InvalidId_ReturnsBadRequest(int invalidId)
    {
        // Act
        var response = await _client.GetAsync($"/api/tickets/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region POST /api/tickets

    [Fact]
    public async Task CreateTicket_ValidData_ReturnsCreated()
    {
        // Arrange
        var request = new CreateTicketRequest
        {
            Title = "New Test Ticket",
            Description = "Description for test ticket",
            Priority = "High"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<TicketResponse>();
        result.Should().NotBeNull();
        result!.Title.Should().Be(request.Title);
        result.Description.Should().Be(request.Description);
        result.Priority.Should().Be(request.Priority);
        result.Status.Should().Be("Open");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateTicket_InvalidTitle_ReturnsBadRequest(string? invalidTitle)
    {
        // Arrange
        var request = new CreateTicketRequest
        {
            Title = invalidTitle!,
            Description = "Valid description",
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateTicket_InvalidDescription_ReturnsBadRequest(string? invalidDescription)
    {
        // Arrange
        var request = new CreateTicketRequest
        {
            Title = "Valid Title",
            Description = invalidDescription!,
            Priority = "Medium"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("InvalidPriority")]
    public async Task CreateTicket_InvalidPriority_ReturnsBadRequest(string? invalidPriority)
    {
        // Arrange
        var request = new CreateTicketRequest
        {
            Title = "Valid Title",
            Description = "Valid description",
            Priority = invalidPriority!
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PUT /api/tickets/{id}

    [Fact]
    public async Task UpdateTicket_ValidData_ReturnsOk()
    {
        // Arrange - Create a ticket first
        var createRequest = new CreateTicketRequest
        {
            Title = "Original Ticket",
            Description = "Original Description",
            Priority = "Medium"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<TicketResponse>();

        var updateRequest = new UpdateTicketRequest
        {
            Title = "Updated Ticket",
            Description = "Updated Description",
            Priority = "High",
            Status = "InProgress"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/tickets/{createdTicket!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TicketResponse>();
        result.Should().NotBeNull();
        result!.Title.Should().Be(updateRequest.Title);
        result.Description.Should().Be(updateRequest.Description);
        result.Priority.Should().Be(updateRequest.Priority);
        result.Status.Should().Be(updateRequest.Status);
    }

    [Fact]
    public async Task UpdateTicket_NonExistentTicket_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;
        var updateRequest = new UpdateTicketRequest
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Priority = "Medium",
            Status = "InProgress"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/tickets/{nonExistentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateTicket_InvalidTitle_ReturnsBadRequest(string? invalidTitle)
    {
        // Arrange - Create a ticket first
        var createRequest = new CreateTicketRequest
        {
            Title = "Original Ticket",
            Description = "Original Description",
            Priority = "Medium"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<TicketResponse>();

        var updateRequest = new UpdateTicketRequest
        {
            Title = invalidTitle!,
            Description = "Valid description",
            Priority = "Medium",
            Status = "Open"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/tickets/{createdTicket!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region DELETE /api/tickets/{id}

    [Fact]
    public async Task DeleteTicket_ExistingTicket_ReturnsNoContent()
    {
        // Arrange - Create a ticket first
        var createRequest = new CreateTicketRequest
        {
            Title = "Ticket to Delete",
            Description = "This ticket will be deleted",
            Priority = "Low"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/tickets/{createdTicket!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTicket_NonExistentTicket_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.DeleteAsync($"/api/tickets/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region GET /api/tickets/my-tickets

    [Fact]
    public async Task GetMyTickets_ValidRequest_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets/my-tickets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResponse<TicketResponse>>();
        result.Should().NotBeNull();
    }

    #endregion

    #region GET /api/tickets/stats

    [Fact]
    public async Task GetTicketStats_ValidRequest_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets/stats");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TicketStatsResponse>();
        result.Should().NotBeNull();
    }

    #endregion

    #region GET /api/tickets/{id}/history

    [Fact]
    public async Task GetTicketHistory_ExistingTicket_ReturnsOk()
    {
        // Arrange - Create a ticket first
        var createRequest = new CreateTicketRequest
        {
            Title = "Ticket with History",
            Description = "Test ticket for history",
            Priority = "Medium"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<TicketResponse>();

        // Act
        var response = await _client.GetAsync($"/api/tickets/{createdTicket!.Id}/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<TicketHistoryResponse>>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTicketHistory_NonExistentTicket_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.GetAsync($"/api/tickets/{nonExistentId}/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}