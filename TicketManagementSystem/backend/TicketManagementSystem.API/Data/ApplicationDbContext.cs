using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Data
{
    /// <summary>
    /// Application DbContext for Entity Framework Core
    /// Configures the database schema and seed data for the ticket management system
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<TicketHistory> TicketHistories { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlite(connectionString);

                // Enable sensitive data logging only in Development environment
                // WARNING: Never enable in production!
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    optionsBuilder.EnableSensitiveDataLogging();
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                // Unique index on Email
                entity.HasIndex(u => u.Email).IsUnique();

                // Index on Role
                entity.HasIndex(u => u.Role);

                // Global query filter for soft delete
                entity.HasQueryFilter(u => !u.IsDeleted);

                // Relationships
                entity.HasMany(u => u.CreatedTickets)
                    .WithOne(t => t.CreatedBy)
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.AssignedTickets)
                    .WithOne(t => t.AssignedTo)
                    .HasForeignKey(t => t.AssignedToId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Ticket configurations
            modelBuilder.Entity<Ticket>(entity =>
            {
                // Individual indexes
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.Priority);
                entity.HasIndex(t => t.CreatedById);
                entity.HasIndex(t => t.AssignedToId);
                entity.HasIndex(t => t.CreatedAt);

                // Composite index for combined filters
                entity.HasIndex(t => new { t.Status, t.Priority });

                // Global query filter for soft delete
                entity.HasQueryFilter(t => !t.IsDeleted);

                // Relationships
                entity.HasMany(t => t.Comments)
                    .WithOne(c => c.Ticket)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.History)
                    .WithOne(h => h.Ticket)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Comment configurations
            modelBuilder.Entity<Comment>(entity =>
            {
                // Composite index on TicketId and CreatedAt (descending)
                entity.HasIndex(c => new { c.TicketId, c.CreatedAt })
                    .IsDescending(false, true);

                // Global query filter for soft delete
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Relationship with User
                entity.HasOne(c => c.CreatedBy)
                    .WithMany(u => u.Comments)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TicketHistory configurations
            modelBuilder.Entity<TicketHistory>(entity =>
            {
                // Composite index on TicketId and ChangedAt (descending)
                entity.HasIndex(h => new { h.TicketId, h.ChangedAt })
                    .IsDescending(false, true);

                // Index on ChangedBy for reports
                entity.HasIndex(h => h.ChangedById);

                // Global query filter to match User's soft delete filter
                entity.HasQueryFilter(h => h.ChangedBy == null || !h.ChangedBy.IsDeleted);
            });

            // Role configurations
            modelBuilder.Entity<Role>(entity =>
            {
                // Unique index on RoleName
                entity.HasIndex(r => r.RoleName).IsUnique();

                // Global query filter for soft delete
                entity.HasQueryFilter(r => !r.IsDeleted);
            });


            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Users
            var adminId = 1;
            var agent1Id = 2;
            var agent2Id = 3;
            var userId = 4;

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    Email = "admin@tickets.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), // Hashed password
                    FullName = "System Administrator",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new User
                {
                    Id = agent1Id,
                    Email = "agent1@tickets.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Agent123!"),
                    FullName = "Agent One",
                    Role = "Agent",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new User
                {
                    Id = agent2Id,
                    Email = "agent2@tickets.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Agent123!"),
                    FullName = "Agent Two",
                    Role = "Agent",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new User
                {
                    Id = userId,
                    Email = "user@tickets.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    FullName = "Regular User",
                    Role = "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    RoleName = "Admin",
                    Description = "System administrator with full access",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Role
                {
                    Id = 2,
                    RoleName = "Agent",
                    Description = "Support agent with ticket management access",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Role
                {
                    Id = 3,
                    RoleName = "User",
                    Description = "Regular user with basic access",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );


            // Seed Tickets
            var ticket1Id = 1;
            var ticket2Id = 2;
            var ticket3Id = 3;

            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = ticket1Id,
                    Title = "Login page not loading",
                    Description = "Users are unable to access the login page. Error 500 is displayed.",
                    Status = Status.Open,
                    Priority = Priority.Critical,
                    CreatedById = userId,
                    AssignedToId = agent1Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    IsDeleted = false
                },
                new Ticket
                {
                    Id = ticket2Id,
                    Title = "Email notifications not working",
                    Description = "System is not sending email notifications for ticket updates.",
                    Status = Status.InProgress,
                    Priority = Priority.High,
                    CreatedById = userId,
                    AssignedToId = agent2Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddHours(-6),
                    IsDeleted = false
                },
                new Ticket
                {
                    Id = ticket3Id,
                    Title = "UI improvement suggestion",
                    Description = "Add dark mode toggle to the user interface.",
                    Status = Status.Resolved,
                    Priority = Priority.Medium,
                    CreatedById = userId,
                    AssignedToId = null,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddHours(-12),
                    IsDeleted = false
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "I've attached screenshots showing the error.",
                    CreatedById = userId,
                    TicketId = ticket1Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2).AddHours(1),
                    IsDeleted = false
                },
                new Comment
                {
                    Id = 2,
                    Content = "Working on reproducing the issue in our test environment.",
                    CreatedById = agent1Id,
                    TicketId = ticket1Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1).AddHours(2),
                    IsDeleted = false
                }
            );

            // Seed Ticket History
            modelBuilder.Entity<TicketHistory>().HasData(
                new TicketHistory
                {
                    Id = 1,
                    TicketId = ticket1Id,
                    ChangedById = userId,
                    ChangedAt = DateTime.UtcNow.AddDays(-2),
                    OldStatus = null,
                    NewStatus = Status.Open,
                    OldPriority = null,
                    NewPriority = Priority.Critical,
                    OldAssignedToId = null,
                    NewAssignedToId = null,
                    ChangeDescription = "Ticket created"
                },
                new TicketHistory
                {
                    Id = 2,
                    TicketId = ticket1Id,
                    ChangedById = adminId,
                    ChangedAt = DateTime.UtcNow.AddDays(-1),
                    OldStatus = Status.Open,
                    NewStatus = Status.Open,
                    OldPriority = Priority.Critical,
                    NewPriority = Priority.Critical,
                    OldAssignedToId = null,
                    NewAssignedToId = agent1Id,
                    ChangeDescription = "Assigned to Agent One"
                },
                new TicketHistory
                {
                    Id = 3,
                    TicketId = ticket2Id,
                    ChangedById = userId,
                    ChangedAt = DateTime.UtcNow.AddDays(-1),
                    OldStatus = null,
                    NewStatus = Status.Open,
                    OldPriority = null,
                    NewPriority = Priority.High,
                    OldAssignedToId = null,
                    NewAssignedToId = null,
                    ChangeDescription = "Ticket created"
                },
                new TicketHistory
                {
                    Id = 4,
                    TicketId = ticket2Id,
                    ChangedById = adminId,
                    ChangedAt = DateTime.UtcNow.AddHours(-6),
                    OldStatus = Status.Open,
                    NewStatus = Status.InProgress,
                    OldPriority = Priority.High,
                    NewPriority = Priority.High,
                    OldAssignedToId = null,
                    NewAssignedToId = agent2Id,
                    ChangeDescription = "Started working on the issue"
                }
            );
        }
    }
}