using FluentAssertions;
using BookingSystemAPI.Api.Common.Responses;

namespace BookingSystemAPI.Tests.Common;

/// <summary>
/// Tests unitarios para la clase ApiResponse.
/// </summary>
public class ApiResponseTests
{
    [Fact]
    public void Ok_DebeCrearRespuestaExitosa_ConDatos()
    {
        // Arrange
        var data = new { Id = 1, Name = "Test" };
        var message = "Operación exitosa";
        var correlationId = Guid.NewGuid().ToString();

        // Act
        var response = ApiResponse<object>.Ok(data, message, correlationId);

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().Be(data);
        response.Message.Should().Be(message);
        response.CorrelationId.Should().Be(correlationId);
        response.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Fail_DebeCrearRespuestaDeError_SinDatos()
    {
        // Arrange
        var errorMessage = "Error de validación";
        var correlationId = Guid.NewGuid().ToString();

        // Act
        var response = ApiResponse<object>.Fail(errorMessage, correlationId);

        // Assert
        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Message.Should().Be(errorMessage);
        response.CorrelationId.Should().Be(correlationId);
    }

    [Fact]
    public void PagedResponse_DebeCalcularPaginacionCorrectamente()
    {
        // Arrange & Act
        var response = new PagedResponse<string>
        {
            Success = true,
            Data = new[] { "item1", "item2" },
            PageNumber = 2,
            PageSize = 10,
            TotalCount = 25
        };

        // Assert
        response.TotalPages.Should().Be(3);
        response.HasPreviousPage.Should().BeTrue();
        response.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void PagedResponse_PrimeraPagina_NoDebeTenerPaginaAnterior()
    {
        // Arrange & Act
        var response = new PagedResponse<string>
        {
            Success = true,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 25
        };

        // Assert
        response.HasPreviousPage.Should().BeFalse();
        response.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void PagedResponse_UltimaPagina_NoDebeTenerPaginaSiguiente()
    {
        // Arrange & Act
        var response = new PagedResponse<string>
        {
            Success = true,
            PageNumber = 3,
            PageSize = 10,
            TotalCount = 25
        };

        // Assert
        response.HasPreviousPage.Should().BeTrue();
        response.HasNextPage.Should().BeFalse();
    }
}
