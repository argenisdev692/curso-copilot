using Swashbuckle.AspNetCore.Filters;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Controllers.Examples
{
    /// <summary>
    /// Ejemplo de request para login
    /// </summary>
    public class LoginRequestExample : IExamplesProvider<LoginDto>
    {
        /// <summary>
        /// Proporciona un ejemplo de LoginDto
        /// </summary>
        /// <returns>Ejemplo de login</returns>
        public LoginDto GetExamples()
        {
            return new LoginDto
            {
                Email = "usuario@ejemplo.com",
                Password = "Password123!"
            };
        }
    }

    /// <summary>
    /// Ejemplo de response para login exitoso
    /// </summary>
    public class LoginResponseExample : IExamplesProvider<LoginResponseDto>
    {
        /// <summary>
        /// Proporciona un ejemplo de LoginResponseDto
        /// </summary>
        /// <returns>Ejemplo de respuesta de login</returns>
        public LoginResponseDto GetExamples()
        {
            return new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                RefreshToken = "refresh_token_aqui",
                ExpiresAt = DateTime.Now.AddHours(1),
                User = new UserBasicDto
                {
                    Id = 1,
                    Email = "usuario@ejemplo.com",
                    FullName = "Juan Pérez"
                }
            };
        }
    }
}