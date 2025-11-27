using System.IO;
using Serilog;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using TicketManagementSystem.API.Validators;
using TicketManagementSystem.API.Mappings;
using MediatR;
using TicketManagementSystem.API.Middlewares;
using TicketManagementSystem.API.MediatR.Behaviors;
using AspNetCoreRateLimit;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using TicketManagementSystem.API.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with sensitive data sanitization
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Destructure.ByTransforming<LoginDto>(login => new
    {
        login.Email,
        Password = "***REDACTED***"
    })
    .Destructure.ByTransforming<RegisterDto>(register => new
    {
        register.Email,
        register.FullName,
        register.Role,
        Password = "***REDACTED***"
    })
    .Destructure.ByTransforming<LoginResponseDto>(response => new
    {
        response.User,
        Token = "***REDACTED***",
        RefreshToken = "***REDACTED***",
        response.ExpiresAt
    })
    .Destructure.ByTransforming<RegisterResponseDto>(response => new
    {
        response.Message,
        response.User
    })
    .Destructure.ByTransforming<RefreshTokenResponseDto>(response => new
    {
        Token = "***REDACTED***",
        RefreshToken = "***REDACTED***",
        response.ExpiresAt
    })
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.ContainsKey("Password") ||
        logEvent.Properties.ContainsKey("Token") ||
        logEvent.Properties.ContainsKey("AccessToken") ||
        logEvent.Properties.ContainsKey("RefreshToken") ||
        logEvent.Properties.ContainsKey("Secret") ||
        logEvent.Properties.ContainsKey("Key") ||
        logEvent.MessageTemplate.Text.Contains("password", StringComparison.OrdinalIgnoreCase) ||
        logEvent.MessageTemplate.Text.Contains("token", StringComparison.OrdinalIgnoreCase))
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireAdminOrAgentRole", policy =>
        policy.RequireRole("Admin", "Agent"));

    options.AddPolicy("RequireAuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
});

// Register helpers
builder.Services.AddSingleton<CacheHelper>();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(TicketMappingProfile), typeof(RoleMappingProfile));

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketDtoValidator>();

// Add Caching
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

// Add Rate Limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Register services
builder.Services.AddScoped<ITicketMetricsService, TicketMetricsService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ITicketBusinessRules, TicketBusinessRules>();
builder.Services.AddScoped<ITicketMapper, TicketMapper>();
builder.Services.AddScoped<ITicketAssignmentService, TicketAssignmentService>();
builder.Services.AddScoped<IEmailValidationService, EmailValidationService>();
builder.Services.AddScoped<IEmailComposer, EmailComposer>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ITicketAuthorizationService, TicketAuthorizationService>();
builder.Services.AddHostedService<EmailNotificationService>();

// Configure options
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ticket Management System API",
        Version = "v1",
        Description = "API para el sistema de gestión de tickets",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "dev@ticketmanagement.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configurar JWT Authentication en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Agregar ejemplos
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:3000", "https://localhost:3000")
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
              .WithHeaders("Authorization", "Content-Type", "Accept", "X-Requested-With", "X-Client-Version", "X-Timestamp", "X-Correlation-ID")
              .AllowCredentials();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks();

// Add Problem Details
builder.Services.AddProblemDetails();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.EnsureCreated();
        Console.WriteLine("Database created successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Only use HTTPS redirection in production
    app.UseHttpsRedirection();
}

// Use CORS with specific policy
app.UseCors("AllowSpecificOrigins");

// Use Rate Limiting
app.UseIpRateLimiting();

// Use Request Logging Middleware with sensitive data sanitization
app.UseMiddleware<RequestLoggingMiddleware>();

// Add security middleware
// app.UseMiddleware<SecurityMiddleware>();

// Add security headers
app.Use(async (context, next) =>
{
    // Add security headers before response starts
    context.Response.OnStarting(() =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
        
        // Content Security Policy
        context.Response.Headers["Content-Security-Policy"] = 
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline'; " +
            "style-src 'self' 'unsafe-inline'; " +
            "img-src 'self' data: https:; " +
            "font-src 'self'; " +
            "connect-src 'self'; " +
            "frame-ancestors 'none';";
        
        return Task.CompletedTask;
    });
    
    await next();
});

// Use Exception Handler Middleware
app.UseExceptionHandlerMiddleware();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health");

// Security monitoring endpoint
app.MapGet("/api/security/status", () => 
{
    return Results.Ok(new 
    { 
        status = "secure", 
        timestamp = DateTime.UtcNow,
        version = "1.0.0",
        security_features = new[]
        {
            "JWT Authentication",
            "CORS Protection", 
            "Rate Limiting",
            "Security Headers",
            "Input Validation",
            "XSS Protection",
            "SQL Injection Prevention"
        }
    });
});

app.Run();
