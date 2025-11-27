using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TicketManagementSystem.API.Services;
using Xunit;

namespace TicketManagementSystem.API.Tests
{
    public class EmailValidationServiceTests
    {
        private readonly Mock<ILogger<EmailValidationService>> _loggerMock;
        private readonly EmailValidationService _service;

        public EmailValidationServiceTests()
        {
            _loggerMock = new Mock<ILogger<EmailValidationService>>();
            _service = new EmailValidationService(_loggerMock.Object);
        }

        [Fact]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "usuario@example.com";

            // Act
            bool result = _service.IsValidEmail(email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValidEmail_InvalidEmail_NoAtSymbol_ReturnsFalse()
        {
            // Arrange
            string email = "usuarioexample.com";

            // Act
            bool result = _service.IsValidEmail(email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValidEmail_InvalidEmail_NoDomain_ReturnsFalse()
        {
            // Arrange
            string email = "usuario@";

            // Act
            bool result = _service.IsValidEmail(email);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsValidEmail_InvalidEmail_NullOrEmpty_ReturnsFalse(string email)
        {
            // Act
            bool result = _service.IsValidEmail(email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValidEmail_ValidEmail_WithSubdomain_ReturnsTrue()
        {
            // Arrange
            string email = "usuario@mail.example.com";

            // Act
            bool result = _service.IsValidEmail(email);

            // Assert
            result.Should().BeTrue();
        }
    }
}