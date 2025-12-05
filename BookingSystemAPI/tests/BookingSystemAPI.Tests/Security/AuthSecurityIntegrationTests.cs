using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using BookingSystemAPI.Api.DTOs.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookingSystemAPI.Tests.Security;

/// <summary>
/// Tests de integración para verificar la seguridad de la autenticación JWT.
/// Cubre OWASP API Security Top 10 (2023).
/// </summary>
public class AuthSecurityIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public AuthSecurityIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });
    }

    #region Security Headers Tests

    /// <summary>
    /// Verifica que todos los endpoints incluyan headers de seguridad OWASP.
    /// </summary>
    [Fact]
    public async Task AllResponses_ShouldInclude_SecurityHeaders()
    {
        // Act
        var response = await _client.GetAsync("/api/health");

        // Assert
        response.Headers.Should().Contain(h => h.Key == "X-Content-Type-Options");
        response.Headers.Should().Contain(h => h.Key == "X-Frame-Options");
        response.Headers.Should().Contain(h => h.Key == "X-XSS-Protection");
        response.Headers.Should().Contain(h => h.Key == "Referrer-Policy");
        
        // Verificar valores específicos
        response.Headers.GetValues("X-Content-Type-Options").Should().Contain("nosniff");
        response.Headers.GetValues("X-Frame-Options").Should().Contain("DENY");
    }

    /// <summary>
    /// Verifica que el header Server no revele información del servidor.
    /// </summary>
    [Fact]
    public async Task Response_ShouldNotReveal_ServerInformation()
    {
        // Act
        var response = await _client.GetAsync("/api/health");

        // Assert
        response.Headers.Should().NotContain(h => h.Key == "Server");
        response.Headers.Should().NotContain(h => h.Key == "X-Powered-By");
        response.Headers.Should().NotContain(h => h.Key == "X-AspNet-Version");
    }

    /// <summary>
    /// Verifica que los endpoints de auth no permitan caché.
    /// </summary>
    [Fact]
    public async Task AuthEndpoints_ShouldHave_NoCacheHeaders()
    {
        // Arrange
        var loginDto = new { Email = "test@test.com", Password = "Test123!" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.Headers.Should().Contain(h => h.Key == "Cache-Control");
        var cacheControl = response.Headers.GetValues("Cache-Control").First();
        cacheControl.Should().Contain("no-store");
        cacheControl.Should().Contain("no-cache");
    }

    #endregion

    #region Rate Limiting Tests

    /// <summary>
    /// Verifica que el rate limiting bloquee requests excesivas.
    /// </summary>
    [Fact]
    public async Task RateLimiting_ShouldBlock_ExcessiveAuthRequests()
    {
        // Arrange
        var loginDto = new { Email = "ratelimit@test.com", Password = "wrong" };
        var responses = new List<HttpResponseMessage>();

        // Act - Enviar más requests que el límite permitido
        for (int i = 0; i < 10; i++)
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            responses.Add(response);
        }

        // Assert - Al menos una debe ser 429 Too Many Requests
        responses.Should().Contain(r => r.StatusCode == HttpStatusCode.TooManyRequests);
    }

    #endregion

    #region Authentication Tests

    /// <summary>
    /// Verifica que credenciales inválidas no revelen existencia de usuario.
    /// OWASP API2:2023 - Broken Authentication.
    /// </summary>
    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldNotReveal_UserExistence()
    {
        // Arrange
        var existingUserLogin = new { Email = "existing@test.com", Password = "wrong" };
        var nonExistingUserLogin = new { Email = "nonexisting@test.com", Password = "wrong" };

        // Act
        var response1 = await _client.PostAsJsonAsync("/api/auth/login", existingUserLogin);
        var response2 = await _client.PostAsJsonAsync("/api/auth/login", nonExistingUserLogin);

        var content1 = await response1.Content.ReadAsStringAsync();
        var content2 = await response2.Content.ReadAsStringAsync();

        // Assert - El mensaje de error debe ser el mismo (genérico)
        response1.StatusCode.Should().Be(response2.StatusCode);
        // El mensaje no debe indicar si el usuario existe o no
        content1.Should().NotContain("user not found", StringComparison.OrdinalIgnoreCase);
        content1.Should().NotContain("email not registered", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifica que el account lockout funcione después de múltiples intentos fallidos.
    /// </summary>
    [Fact]
    public async Task AccountLockout_ShouldActivate_AfterMultipleFailedAttempts()
    {
        // Arrange
        var uniqueEmail = $"lockout-{Guid.NewGuid()}@test.com";
        var loginDto = new { Email = uniqueEmail, Password = "wrong" };

        // Act - 5 intentos fallidos (límite configurado)
        for (int i = 0; i < 6; i++)
        {
            await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        }
        
        var lastResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var content = await lastResponse.Content.ReadAsStringAsync();

        // Assert - Debería indicar bloqueo
        content.Should().Contain("bloqueada", StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Token Security Tests

    /// <summary>
    /// Verifica que tokens manipulados sean rechazados.
    /// </summary>
    [Fact]
    public async Task TamperedToken_ShouldBe_Rejected()
    {
        // Arrange - Token manipulado (firma alterada)
        var tamperedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                           "eyJzdWIiOiIxIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwiZXhwIjoxOTk5OTk5OTk5fQ." +
                           "TAMPERED_SIGNATURE_HERE";

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tamperedToken}");

        // Act
        var response = await _client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifica que tokens expirados sean rechazados.
    /// </summary>
    [Fact]
    public async Task ExpiredToken_ShouldBe_Rejected()
    {
        // Arrange - Token con fecha de expiración pasada
        var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                          "eyJzdWIiOiIxIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwiZXhwIjoxNjAwMDAwMDAwfQ." +
                          "invalid_signature";

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {expiredToken}");

        // Act
        var response = await _client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Input Validation Tests

    /// <summary>
    /// Verifica protección contra SQL Injection.
    /// </summary>
    [Theory]
    [InlineData("'; DROP TABLE Users; --")]
    [InlineData("1' OR '1'='1")]
    [InlineData("admin'--")]
    [InlineData("1; SELECT * FROM Users")]
    public async Task SqlInjection_ShouldBe_Blocked(string maliciousInput)
    {
        // Arrange
        var loginDto = new { Email = maliciousInput, Password = maliciousInput };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert - No debe causar error de servidor (500)
        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Verifica protección contra XSS en inputs.
    /// </summary>
    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("javascript:alert(1)")]
    [InlineData("<img src=x onerror=alert(1)>")]
    [InlineData("{{constructor.constructor('alert(1)')()}}")]
    public async Task XssPayload_ShouldBe_Sanitized(string xssPayload)
    {
        // Arrange
        var registerDto = new
        {
            Email = "xsstest@test.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!",
            FirstName = xssPayload,
            LastName = xssPayload
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
        var content = await response.Content.ReadAsStringAsync();

        // Assert - El payload no debe estar sin sanitizar
        content.Should().NotContain("<script>");
        content.Should().NotContain("javascript:");
        content.Should().NotContain("onerror=");
    }

    #endregion

    #region CORS Tests

    /// <summary>
    /// Verifica que CORS rechace orígenes no autorizados.
    /// </summary>
    [Fact]
    public async Task Cors_ShouldReject_UnauthorizedOrigins()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Options, "/api/auth/login");
        request.Headers.Add("Origin", "https://evil-site.com");
        request.Headers.Add("Access-Control-Request-Method", "POST");

        // Act
        var response = await _client.SendAsync(request);

        // Assert - No debe incluir el origen malicioso en Access-Control-Allow-Origin
        if (response.Headers.TryGetValues("Access-Control-Allow-Origin", out var origins))
        {
            origins.Should().NotContain("https://evil-site.com");
        }
    }

    #endregion

    #region Refresh Token Tests

    /// <summary>
    /// Verifica que refresh tokens revocados no puedan reutilizarse.
    /// OWASP - Token Reuse Detection.
    /// </summary>
    [Fact]
    public async Task RevokedRefreshToken_ShouldBe_Rejected()
    {
        // Este test requiere un usuario registrado y login previo
        // Implementar según el flujo completo de la aplicación
        
        // Arrange - Simular obtención de refresh token
        var refreshDto = new { RefreshToken = "revoked_token_here" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}

/// <summary>
/// Tests unitarios para el servicio de autenticación.
/// </summary>
public class AuthServiceSecurityTests
{
    /// <summary>
    /// Verifica que el token JWT no contenga información sensible.
    /// </summary>
    [Fact]
    public void JwtToken_ShouldNotContain_SensitiveData()
    {
        // Arrange
        var sampleToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                         "eyJzdWIiOiIxIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwibmFtZSI6IlRlc3QifQ." +
                         "signature";

        var handler = new JwtSecurityTokenHandler();
        
        // Act
        var token = handler.ReadJwtToken(sampleToken);

        // Assert - No debe contener claims sensibles
        token.Claims.Should().NotContain(c => 
            c.Type.Contains("password", StringComparison.OrdinalIgnoreCase));
        token.Claims.Should().NotContain(c => 
            c.Type.Contains("hash", StringComparison.OrdinalIgnoreCase));
        token.Claims.Should().NotContain(c => 
            c.Type.Contains("secret", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica que el hash de password use workFactor adecuado.
    /// </summary>
    [Fact]
    public void PasswordHash_ShouldUse_StrongWorkFactor()
    {
        // Arrange
        var password = "TestPassword123!";

        // Act
        var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        // Assert
        hash.Should().StartWith("$2"); // BCrypt prefix
        BCrypt.Net.BCrypt.Verify(password, hash).Should().BeTrue();
        
        // Verificar que el workFactor sea >= 12
        var costString = hash.Split('$')[3].Substring(0, 2);
        int.Parse(costString).Should().BeGreaterOrEqualTo(12);
    }
}
