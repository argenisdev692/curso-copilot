using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using BookingSystemAPI.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BookingSystemAPI.Api.Common.Responses;

namespace BookingSystemAPI.Tests.Controllers;

/// <summary>
/// Tests unitarios para el HealthController.
/// </summary>
public class HealthControllerTests
{
    private readonly ILogger<HealthController> _logger;
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _logger = Substitute.For<ILogger<HealthController>>();
        _controller = new HealthController(_logger);

        // Configurar HttpContext mock
        var httpContext = new DefaultHttpContext();
        httpContext.Items["CorrelationId"] = Guid.NewGuid().ToString();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public void GetStatus_DebeRetornarOk_ConEstadoHealthy()
    {
        // Arrange - ya configurado en constructor

        // Act
        var result = _controller.GetStatus();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<ApiResponse<object>>();
    }

    [Fact]
    public void GetVersion_DebeRetornarOk_ConInformacionDeVersion()
    {
        // Arrange - ya configurado en constructor

        // Act
        var result = _controller.GetVersion();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
    }
}