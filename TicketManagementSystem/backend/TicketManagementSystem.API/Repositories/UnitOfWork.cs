using TicketManagementSystem.API.Data;
using Microsoft.Extensions.Logging;

namespace TicketManagementSystem.API.Repositories;

/// <summary>
/// Interface for Unit of Work pattern
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITicketRepository Tickets { get; }
    ICommentRepository Comments { get; }
    IRoleRepository Roles { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

/// <summary>
/// Implementation of Unit of Work
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(
        ApplicationDbContext context,
        ILogger<UnitOfWork> logger,
        IUserRepository users,
        ITicketRepository tickets,
        ICommentRepository comments,
        IRoleRepository roles)
    {
        _context = context;
        _logger = logger;
        Users = users;
        Tickets = tickets;
        Comments = comments;
        Roles = roles;
    }

    public IUserRepository Users { get; }
    public ITicketRepository Tickets { get; }
    public ICommentRepository Comments { get; }
    public IRoleRepository Roles { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}