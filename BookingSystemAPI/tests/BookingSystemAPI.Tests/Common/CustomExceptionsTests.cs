using FluentAssertions;
using BookingSystemAPI.Api.Common.Exceptions;

namespace BookingSystemAPI.Tests.Common;

/// <summary>
/// Tests unitarios para las excepciones personalizadas.
/// </summary>
public class CustomExceptionsTests
{
    [Fact]
    public void NotFoundException_DebeContenerInformacionDelRecurso()
    {
        // Arrange
        var resourceName = "Booking";
        var resourceId = 123;

        // Act
        var exception = new NotFoundException(resourceName, resourceId);

        // Assert
        exception.ResourceName.Should().Be(resourceName);
        exception.ResourceId.Should().Be(resourceId);
        exception.Message.Should().Contain(resourceName);
        exception.Message.Should().Contain(resourceId.ToString());
    }

    [Fact]
    public void BusinessException_DebeContenerCodigoDeError()
    {
        // Arrange
        var message = "No se puede cancelar una reserva confirmada";
        var errorCode = "BOOKING_CANCEL_ERROR";

        // Act
        var exception = new BusinessException(message, errorCode);

        // Assert
        exception.Message.Should().Be(message);
        exception.ErrorCode.Should().Be(errorCode);
    }

    [Fact]
    public void BusinessException_DebeUsarCodigoPorDefecto_CuandoNoSeEspecifica()
    {
        // Arrange
        var message = "Error de negocio genérico";

        // Act
        var exception = new BusinessException(message);

        // Assert
        exception.ErrorCode.Should().Be("BUSINESS_ERROR");
    }
}
