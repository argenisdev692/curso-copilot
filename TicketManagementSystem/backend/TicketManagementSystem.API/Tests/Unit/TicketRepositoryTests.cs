using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using Xunit;

namespace TicketManagementSystem.API.Tests.Unit;

/// <summary>
/// Unit tests for TicketRepository using InMemory Database
/// </summary>
public class TicketRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TicketRepository _repository;
    private readonly ILogger<TicketRepository> _logger;

    public TicketRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _logger = new LoggerFactory().CreateLogger<TicketRepository>();
        _repository = new TicketRepository(_context, _logger);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var user1 = new User { Id = 1, Email = "user1@test.com", FullName = "User One" };
        var user2 = new User { Id = 2, Email = "user2@test.com", FullName = "User Two" };

        var priorityLow = new Priority { Id = 1, Name = "Low", Level = 1 };
        var priorityHigh = new Priority { Id = 2, Name = "High", Level = 3 };

        _context.Users.AddRange(user1, user2);
        _context.Priorities.AddRange(priorityLow, priorityHigh);

        var tickets = new[]
        {
            new Ticket
            {
                Id = 1,
                Title = "First Ticket",
                Description = "Description for first ticket",
                Status = Status.Open,
                Priority = Priority.Low,
                CreatedById = 1,
                AssignedToId = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Ticket
            {
                Id = 2,
                Title = "Second Ticket",
                Description = "Description for second ticket",
                Status = Status.InProgress,
                Priority = Priority.High,
                CreatedById = 2,
                AssignedToId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            },
            new Ticket
            {
                Id = 3,
                Title = "Third Ticket",
                Description = "Another ticket description",
                Status = Status.Closed,
                Priority = Priority.Medium,
                CreatedById = 1,
                AssignedToId = null,
                CreatedAt = DateTime.UtcNow.AddHours(-5),
                UpdatedAt = DateTime.UtcNow.AddHours(-1)
            }
        };

        _context.Tickets.AddRange(tickets);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsTicket()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetByIdAsync(ticketId, ct);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(ticketId);
        result.Title.Should().Be("First Ticket");
        result.CreatedBy.Should().NotBeNull();
        result.AssignedTo.Should().NotBeNull();
        result.Priority.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        var nonExistentId = 999;
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId, ct);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithRelations_IncludesRelatedEntities()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetByIdAsync(ticketId, true, ct);

        // Assert
        result.Should().NotBeNull();
        result!.CreatedBy.Should().NotBeNull();
        result.CreatedBy!.FullName.Should().Be("User One");
        result.AssignedTo.Should().NotBeNull();
        result.AssignedTo!.FullName.Should().Be("User Two");
        result.Priority.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithoutRelations_ExcludesRelatedEntities()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetByIdAsync(ticketId, false, ct);

        // Assert
        result.Should().NotBeNull();
        // When not including relations, navigation properties should be null
        result!.CreatedBy.Should().BeNull();
        result.AssignedTo.Should().BeNull();
        result.Priority.Should().BeNull();
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_NoFilters_ReturnsAllTickets()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalItems.Should().Be(3);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsPagedResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 2
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalItems.Should().Be(3);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task GetAllAsync_WithStatusFilter_ReturnsFilteredResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            Status = "Open"
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Status.Should().Be(Status.Open);
        result.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_WithPriorityFilter_ReturnsFilteredResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            PriorityId = 1 // Low priority
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Priority.Should().Be(Priority.Low);
        result.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_WithAssignedToFilter_ReturnsFilteredResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            AssignedTo = 1
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().AssignedToId.Should().Be(1);
        result.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_WithSearchFilter_ReturnsFilteredResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            Search = "First"
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Title.Should().Contain("First");
        result.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_WithSortingByCreatedAtDesc_ReturnsSortedResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            SortBy = "createdat",
            SortOrder = "desc"
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        // Should be ordered by CreatedAt descending (newest first)
        result.Items[0].Id.Should().Be(2); // Most recent
        result.Items[1].Id.Should().Be(3);
        result.Items[2].Id.Should().Be(1); // Oldest
    }

    [Fact]
    public async Task GetAllAsync_WithSortingByPriorityAsc_ReturnsSortedResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            SortBy = "priority",
            SortOrder = "asc"
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        // Should be ordered by Priority ascending
        result.Items[0].Priority.Should().Be(Priority.Low);
        result.Items[1].Priority.Should().Be(Priority.Medium);
        result.Items[2].Priority.Should().Be(Priority.High);
    }

    #endregion

    #region AddAsync

    [Fact]
    public async Task AddAsync_ValidTicket_AddsAndReturnsTicket()
    {
        // Arrange
        var newTicket = new Ticket
        {
            Title = "New Test Ticket",
            Description = "Test description",
            Status = Status.Open,
            Priority = Priority.Medium,
            CreatedById = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.AddAsync(newTicket, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(newTicket);
        result.Id.Should().BeGreaterThan(0);

        // Verify it was actually saved
        var savedTicket = await _context.Tickets.FindAsync(result.Id);
        savedTicket.Should().NotBeNull();
        savedTicket!.Title.Should().Be("New Test Ticket");
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ExistingTicket_UpdatesTicket()
    {
        // Arrange
        var ticketId = 1;
        var ticket = await _context.Tickets.FindAsync(ticketId);
        ticket!.Title = "Updated Title";
        ticket.UpdatedAt = DateTime.UtcNow;
        var ct = CancellationToken.None;

        // Act
        await _repository.UpdateAsync(ticket, ct);

        // Assert
        var updatedTicket = await _context.Tickets.FindAsync(ticketId);
        updatedTicket.Should().NotBeNull();
        updatedTicket!.Title.Should().Be("Updated Title");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task GetAllAsync_PageBeyondTotal_ReturnsEmptyResults()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 10, // Way beyond total pages
            PageSize = 10
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItems.Should().Be(3);
        result.Page.Should().Be(10);
    }

    [Fact]
    public async Task GetAllAsync_InvalidStatusFilter_IgnoresFilter()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            Status = "InvalidStatus"
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3); // All tickets returned since filter is ignored
        result.TotalItems.Should().Be(3);
    }

    [Fact]
    public async Task GetAllAsync_CaseInsensitiveSearch_Works()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10,
            Search = "TICKET" // Mixed case
        };
        var ct = CancellationToken.None;

        // Act
        var result = await _repository.GetAllAsync(parameters, ct);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3); // All tickets contain "ticket" case-insensitively
        result.TotalItems.Should().Be(3);
    }

    #endregion
}