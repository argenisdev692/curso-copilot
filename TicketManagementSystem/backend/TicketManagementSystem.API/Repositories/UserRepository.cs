using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking() // Use AsNoTracking for read operations
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Use compiled query for better performance
            return await CompiledQueries.GetUserByEmailAsync(_context, email);
        }

        public async Task<PagedResponse<User>> GetUsersAsync(GetUsersQueryParameters parameters)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(search) ||
                                        u.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrEmpty(parameters.Role))
            {
                query = query.Where(u => u.Role == parameters.Role);
            }

            if (parameters.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == parameters.IsActive.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = parameters.SortBy?.ToLower() switch
            {
                "fullname" => parameters.SortOrder?.ToLower() == "desc" 
                    ? query.OrderByDescending(u => u.FullName) 
                    : query.OrderBy(u => u.FullName),
                "email" => parameters.SortOrder?.ToLower() == "desc" 
                    ? query.OrderByDescending(u => u.Email) 
                    : query.OrderBy(u => u.Email),
                "role" => parameters.SortOrder?.ToLower() == "desc" 
                    ? query.OrderByDescending(u => u.Role) 
                    : query.OrderBy(u => u.Role),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            // Apply pagination
            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResponse<User>
            {
                Items = items,
                TotalItems = totalCount,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            // Use compiled query for better performance
            return await CompiledQueries.UserExistsAndActiveAsync(_context, id);
        }
    }
}