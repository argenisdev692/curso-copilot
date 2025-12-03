using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación del repositorio de usuarios.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context, ILogger<Repository<User>> logger)
        : base(context, logger)
    {
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    /// <inheritdoc />
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && u.IsActive);
    }
}