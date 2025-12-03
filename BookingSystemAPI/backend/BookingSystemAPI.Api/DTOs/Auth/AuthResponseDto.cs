namespace BookingSystemAPI.Api.DTOs.Auth;

/// <summary>
/// DTO para la respuesta de autenticación.
/// </summary>
/// <example>
/// {
///     "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
///     "refreshToken": "abc123xyz...",
///     "expiresAt": "2025-12-02T15:30:00Z",
///     "user": {
///         "id": 1,
///         "email": "usuario@ejemplo.com",
///         "firstName": "Juan",
///         "lastName": "Pérez"
///     }
/// }
/// </example>
public record AuthResponseDto
{
    /// <summary>
    /// Token JWT de acceso.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJ1c3VhcmlvQGVqZW1wbG8uY29tIn0.xyz</example>
    public required string Token { get; init; }

    /// <summary>
    /// Token de refresco para renovar el token de acceso.
    /// </summary>
    /// <example>dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4gZXhhbXBsZQ==</example>
    public string? RefreshToken { get; init; }

    /// <summary>
    /// Fecha de expiración del token de acceso.
    /// </summary>
    /// <example>2025-12-02T15:30:00Z</example>
    public required DateTime ExpiresAt { get; init; }

    /// <summary>
    /// Información del usuario autenticado.
    /// </summary>
    public required UserDto User { get; init; }
}

/// <summary>
/// DTO simplificado del usuario para respuestas.
/// </summary>
public record UserDto
{
    /// <summary>
    /// ID del usuario.
    /// </summary>
    /// <example>1</example>
    public required int Id { get; init; }

    /// <summary>
    /// Correo electrónico.
    /// </summary>
    /// <example>usuario@ejemplo.com</example>
    public required string Email { get; init; }

    /// <summary>
    /// Nombre.
    /// </summary>
    /// <example>Juan</example>
    public required string FirstName { get; init; }

    /// <summary>
    /// Apellido.
    /// </summary>
    /// <example>Pérez</example>
    public required string LastName { get; init; }
}