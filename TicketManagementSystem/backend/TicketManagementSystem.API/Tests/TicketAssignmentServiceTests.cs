using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;
using Xunit;

namespace TicketManagementSystem.API.Tests
{
    public class TicketAssignmentServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<TicketAssignmentService>> _loggerMock;
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly TicketAssignmentService _service;

        public TicketAssignmentServiceTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<TicketAssignmentService>>();
            _contextMock = new Mock<ApplicationDbContext>();

            _service = new TicketAssignmentService(
                _contextMock.Object,
                _ticketRepositoryMock.Object,
                _userRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task AssignTicketAsync_ValidUserAndTicket_ReturnsAssignedTicket()
        {
            // Arrange
            var ticketId = 1;
            var userId = 2;
            var ticket = new Ticket
            {
                Id = ticketId,
                Title = "Test Ticket",
                Description = "Test Description",
                Status = Status.Open,
                AssignedToId = null
            };
            var user = new User
            {
                Id = userId,
                FullName = "Test Agent",
                Role = "Agent"
            };

            _ticketRepositoryMock.Setup(r => r.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.AssignTicketAsync(ticketId, userId, CancellationToken.None);

            // Assert
            result.Should().Be(ticket);
            result.AssignedToId.Should().Be(userId);
            _ticketRepositoryMock.Verify(r => r.GetByIdAsync(ticketId), Times.Once);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task AssignTicketAsync_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var ticketId = 1;
            var userId = 2;
            var ticket = new Ticket
            {
                Id = ticketId,
                Title = "Test Ticket",
                Description = "Test Description",
                Status = Status.Open
            };

            _ticketRepositoryMock.Setup(r => r.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _service.AssignTicketAsync(ticketId, userId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Usuario no encontrado.");
        }

        [Fact]
        public async Task AssignTicketAsync_TicketClosed_ThrowsInvalidOperationException()
        {
            // Arrange
            var ticketId = 1;
            var userId = 2;
            var ticket = new Ticket
            {
                Id = ticketId,
                Title = "Test Ticket",
                Description = "Test Description",
                Status = Status.Closed
            };
            var user = new User
            {
                Id = userId,
                FullName = "Test Agent",
                Role = "Agent"
            };

            _ticketRepositoryMock.Setup(r => r.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _service.AssignTicketAsync(ticketId, userId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se puede asignar un ticket cerrado.");
        }

        [Fact]
        public async Task AssignTicketAsync_UserNotAgent_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var ticketId = 1;
            var userId = 2;
            var ticket = new Ticket
            {
                Id = ticketId,
                Title = "Test Ticket",
                Description = "Test Description",
                Status = Status.Open
            };
            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                Role = "User"
            };

            _ticketRepositoryMock.Setup(r => r.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _service.AssignTicketAsync(ticketId, userId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Solo los agentes pueden ser asignados a tickets.");
        }
    }
}