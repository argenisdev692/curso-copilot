using BookingSystemAPI.Api.Models;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Interfaz para el repositorio de usuarios.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Obtiene un usuario por su correo electrónico.
    /// </summary>
    /// <param name="email">Correo electrónico del usuario.</param>
    /// <returns>El usuario si existe, null en caso contrario.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Verifica si un correo electrónico ya está registrado.
    /// </summary>
    /// <param name="email">Correo electrónico a verificar.</param>
    /// <returns>True si el correo ya existe, false en caso contrario.</returns>
    Task<bool> EmailExistsAsync(string email);
}