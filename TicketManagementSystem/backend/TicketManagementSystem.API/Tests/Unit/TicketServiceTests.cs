using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;
using Xunit;

namespace TicketManagementSystem.API.Tests.Unit;

/// <summary>
/// Unit tests for TicketService
/// </summary>
public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITicketBusinessRules> _businessRulesMock;
    private readonly Mock<ITicketMapper> _mapperMock;
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<ILogger<TicketService>> _loggerMock;
    private readonly TicketService _ticketService;

    public TicketServiceTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _businessRulesMock = new Mock<ITicketBusinessRules>();
        _mapperMock = new Mock<ITicketMapper>();
        _contextMock = new Mock<ApplicationDbContext>();
        _loggerMock = new Mock<ILogger<TicketService>>();

        _ticketService = new TicketService(
            _ticketRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _businessRulesMock.Object,
            _mapperMock.Object,
            _contextMock.Object,
            _loggerMock.Object);
    }

    #region GetTicketsAsync

    [Fact]
    public async Task GetTicketsAsync_ValidParameters_ReturnsPagedResponse()
    {
        // Arrange
        var parameters = new GetTicketsQueryParameters
        {
            Page = 1,
            PageSize = 10
        };

        var pagedTickets = new PagedResponse<Ticket>
        {
            Items = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Test Ticket" }
            },
            TotalItems = 1,
            Page = 1,
            PageSize = 10
        };

        var ticketDtos = new List<TicketDto>
        {
            new TicketDto { Id = 1, Title = "Test Ticket" }
        };

        _ticketRepositoryMock
            .Setup(x => x.GetAllAsync(parameters, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedTickets);

        _mapperMock
            .Setup(x => x.MapToDtos(It.IsAny<IEnumerable<Ticket>>()))
            .Returns(ticketDtos);

        // Act
        var result = await _ticketService.GetTicketsAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalItems.Should().Be(1);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        _ticketRepositoryMock.Verify(x => x.GetAllAsync(parameters, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.MapToDtos(It.IsAny<IEnumerable<Ticket>>()), Times.Once);
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ValidData_ReturnsCreatedTicket()
    {
        // Arrange
        var dto = new CreateTicketDto
        {
            Title = "New Ticket",
            Description = "Description",
            Priority = "High"
        };
        var createdByUserId = 1;
        var ct = CancellationToken.None;

        var ticketEntity = new Ticket
        {
            Id = 1,
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedById = createdByUserId
        };

        var createdTicket = new Ticket
        {
            Id = 1,
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedById = createdByUserId
        };

        _mapperMock
            .Setup(x => x.MapToEntity(dto, createdByUserId))
            .Returns(ticketEntity);

        _ticketRepositoryMock
            .Setup(x => x.AddAsync(ticketEntity, ct))
            .ReturnsAsync(createdTicket);

        // Act
        var result = await _ticketService.CreateAsync(dto, createdByUserId, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(createdTicket);

        _businessRulesMock.Verify(x => x.ValidateTicketCreationAsync(dto, createdByUserId, ct), Times.Once);
        _mapperMock.Verify(x => x.MapToEntity(dto, createdByUserId), Times.Once);
        _ticketRepositoryMock.Verify(x => x.AddAsync(ticketEntity, ct), Times.Once);
        _businessRulesMock.Verify(x => x.CreateTicketHistoryAsync(createdTicket, "Ticket created", createdByUserId, ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_NullDto_ThrowsArgumentNullException()
    {
        // Arrange
        CreateTicketDto? dto = null;
        var createdByUserId = 1;
        var ct = CancellationToken.None;

        // Act
        var act = () => _ticketService.CreateAsync(dto!, createdByUserId, ct);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("dto");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateAsync_InvalidUserId_ThrowsArgumentException(int invalidUserId)
    {
        // Arrange
        var dto = new CreateTicketDto
        {
            Title = "Test",
            Description = "Test",
            Priority = "Medium"
        };
        var ct = CancellationToken.None;

        // Act
        var act = () => _ticketService.CreateAsync(dto, invalidUserId, ct);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("createdByUserId");
    }

    [Fact]
    public async Task CreateAsync_BusinessRulesValidationFails_ThrowsException()
    {
        // Arrange
        var dto = new CreateTicketDto
        {
            Title = "Test",
            Description = "Test",
            Priority = "Medium"
        };
        var createdByUserId = 1;
        var ct = CancellationToken.None;

        _businessRulesMock
            .Setup(x => x.ValidateTicketCreationAsync(dto, createdByUserId, ct))
            .ThrowsAsync(new InvalidOperationException("Business rule violation"));

        // Act
        var act = () => _ticketService.CreateAsync(dto, createdByUserId, ct);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Business rule violation");

        _businessRulesMock.Verify(x => x.ValidateTicketCreationAsync(dto, createdByUserId, ct), Times.Once);
        _mapperMock.Verify(x => x.MapToEntity(It.IsAny<CreateTicketDto>(), It.IsAny<int>()), Times.Never);
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ValidData_ReturnsUpdatedTicket()
    {
        // Arrange
        var ticketId = 1;
        var dto = new UpdateTicketDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Priority = "High",
            Status = "InProgress"
        };
        var ct = CancellationToken.None;

        var existingTicket = new Ticket
        {
            Id = ticketId,
            Title = "Original Title",
            Description = "Original Description",
            Priority = "Medium",
            Status = "Open",
            AssignedToId = null
        };

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, false, ct))
            .ReturnsAsync(existingTicket);

        // Act
        var result = await _ticketService.UpdateAsync(ticketId, dto, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(existingTicket);

        _businessRulesMock.Verify(x => x.ValidateTicketUpdateAsync(ticketId, dto, ct), Times.Once);
        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, false, ct), Times.Once);
        _mapperMock.Verify(x => x.ApplyUpdate(existingTicket, dto), Times.Once);
        _ticketRepositoryMock.Verify(x => x.UpdateAsync(existingTicket, ct), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateAsync_InvalidId_ThrowsArgumentException(int invalidId)
    {
        // Arrange
        var dto = new UpdateTicketDto
        {
            Title = "Test",
            Description = "Test",
            Priority = "Medium"
        };
        var ct = CancellationToken.None;

        // Act
        var act = () => _ticketService.UpdateAsync(invalidId, dto, ct);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("id");
    }

    [Fact]
    public async Task UpdateAsync_NullDto_ThrowsArgumentNullException()
    {
        // Arrange
        var ticketId = 1;
        UpdateTicketDto? dto = null;
        var ct = CancellationToken.None;

        // Act
        var act = () => _ticketService.UpdateAsync(ticketId, dto!, ct);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("dto");
    }

    [Fact]
    public async Task UpdateAsync_NonExistentTicket_ThrowsKeyNotFoundException()
    {
        // Arrange
        var ticketId = 999;
        var dto = new UpdateTicketDto
        {
            Title = "Test",
            Description = "Test",
            Priority = "Medium"
        };
        var ct = CancellationToken.None;

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, false, ct))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = () => _ticketService.UpdateAsync(ticketId, dto, ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Ticket with ID {ticketId} not found");

        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, false, ct), Times.Once);
        _mapperMock.Verify(x => x.ApplyUpdate(It.IsAny<Ticket>(), It.IsAny<UpdateTicketDto>()), Times.Never);
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ExistingTicket_ReturnsTicketDto()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        var ticket = new Ticket
        {
            Id = ticketId,
            Title = "Test Ticket",
            Description = "Test Description"
        };

        var ticketDto = new TicketDto
        {
            Id = ticketId,
            Title = "Test Ticket",
            Description = "Test Description"
        };

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, true, ct))
            .ReturnsAsync(ticket);

        _mapperMock
            .Setup(x => x.MapToDto(ticket))
            .Returns(ticketDto);

        // Act
        var result = await _ticketService.GetByIdAsync(ticketId, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(ticketDto);

        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, true, ct), Times.Once);
        _mapperMock.Verify(x => x.MapToDto(ticket), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentTicket_ReturnsNull()
    {
        // Arrange
        var ticketId = 999;
        var ct = CancellationToken.None;

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, true, ct))
            .ReturnsAsync((Ticket?)null);

        // Act
        var result = await _ticketService.GetByIdAsync(ticketId, ct);

        // Assert
        result.Should().BeNull();

        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, true, ct), Times.Once);
        _mapperMock.Verify(x => x.MapToDto(It.IsAny<Ticket>()), Times.Never);
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ExistingTicket_SoftDeletesTicket()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        var ticket = new Ticket
        {
            Id = ticketId,
            Title = "Test Ticket",
            IsDeleted = false
        };

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, false, ct))
            .ReturnsAsync(ticket);

        // Act
        await _ticketService.DeleteAsync(ticketId, ct);

        // Assert
        ticket.IsDeleted.Should().BeTrue();
        ticket.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, false, ct), Times.Once);
        _ticketRepositoryMock.Verify(x => x.UpdateAsync(ticket, ct), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAsync_InvalidId_ThrowsArgumentException(int invalidId)
    {
        // Arrange
        var ct = CancellationToken.None;

        // Act
        var act = () => _ticketService.DeleteAsync(invalidId, ct);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("id");
    }

    [Fact]
    public async Task DeleteAsync_NonExistentTicket_ThrowsKeyNotFoundException()
    {
        // Arrange
        var ticketId = 999;
        var ct = CancellationToken.None;

        _ticketRepositoryMock
            .Setup(x => x.GetByIdAsync(ticketId, false, ct))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = () => _ticketService.DeleteAsync(ticketId, ct);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Ticket with ID {ticketId} not found");

        _ticketRepositoryMock.Verify(x => x.GetByIdAsync(ticketId, false, ct), Times.Once);
        _ticketRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region GetUserTicketsAsync

    [Fact]
    public async Task GetUserTicketsAsync_ValidUserId_ReturnsUserTickets()
    {
        // Arrange
        var userId = 1;
        var ct = CancellationToken.None;

        var tickets = new List<Ticket>
        {
            new Ticket
            {
                Id = 1,
                Title = "User Ticket 1",
                CreatedById = userId,
                IsDeleted = false
            },
            new Ticket
            {
                Id = 2,
                Title = "User Ticket 2",
                AssignedToId = userId,
                IsDeleted = false
            }
        };

        var ticketDtos = new List<TicketDto>
        {
            new TicketDto { Id = 1, Title = "User Ticket 1" },
            new TicketDto { Id = 2, Title = "User Ticket 2" }
        };

        var mockDbSet = new Mock<DbSet<Ticket>>();
        mockDbSet.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(tickets.AsQueryable().Provider);
        mockDbSet.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(tickets.AsQueryable().Expression);
        mockDbSet.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(tickets.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(tickets.GetEnumerator());

        _contextMock.Setup(c => c.Tickets).Returns(mockDbSet.Object);
        _mapperMock.Setup(x => x.MapToDtos(tickets)).Returns(ticketDtos);

        // Act
        var result = await _ticketService.GetUserTicketsAsync(userId, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(ticketDtos);
    }

    #endregion

    #region GetTicketHistoryAsync

    [Fact]
    public async Task GetTicketHistoryAsync_ValidTicketId_ReturnsHistory()
    {
        // Arrange
        var ticketId = 1;
        var ct = CancellationToken.None;

        var history = new List<TicketHistory>
        {
            new TicketHistory
            {
                Id = 1,
                TicketId = ticketId,
                ChangeDescription = "Ticket created",
                ChangedAt = DateTime.UtcNow
            }
        };

        var mockDbSet = new Mock<DbSet<TicketHistory>>();
        mockDbSet.As<IQueryable<TicketHistory>>().Setup(m => m.Provider).Returns(history.AsQueryable().Provider);
        mockDbSet.As<IQueryable<TicketHistory>>().Setup(m => m.Expression).Returns(history.AsQueryable().Expression);
        mockDbSet.As<IQueryable<TicketHistory>>().Setup(m => m.ElementType).Returns(history.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<TicketHistory>>().Setup(m => m.GetEnumerator()).Returns(history.GetEnumerator());

        _contextMock.Setup(c => c.TicketHistories).Returns(mockDbSet.Object);

        // Act
        var result = await _ticketService.GetTicketHistoryAsync(ticketId, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().TicketId.Should().Be(ticketId);
    }

    #endregion
}