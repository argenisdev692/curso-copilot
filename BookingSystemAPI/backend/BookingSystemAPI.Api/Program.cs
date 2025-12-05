using System.Threading.RateLimiting;
using BookingSystemAPI.Api;
using BookingSystemAPI.Api.Common.Exceptions;
using BookingSystemAPI.Api.Common.Middleware;
using BookingSystemAPI.Api.Common.Security;
using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

// Configuración de Serilog antes de crear el builder
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando BookingSystemAPI...");

    var builder = WebApplication.CreateBuilder(args);

    // Configurar Serilog desde appsettings
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName());

    // ========================================
    // CONFIGURACIÓN DE SERVICIOS
    // ========================================

    // Configurar DbContext - SQL Server para producción, InMemory para desarrollo
    if (builder.Environment.IsDevelopment())
    {
        // Usar InMemory para desarrollo rápido, o SQL Server LocalDB
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("localdb"))
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("BookingSystemDb"));
            Log.Information("Usando base de datos InMemory para desarrollo");
        }
        else
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            Log.Information("Usando SQL Server para desarrollo");
        }
    }
    else
    {
        // Producción: SQL Server obligatorio
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    // Configurar AutoMapper
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // Registrar HttpContextAccessor para acceso al usuario actual
    builder.Services.AddHttpContextAccessor();

    // Configurar FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    // Registrar servicios de la aplicación
    builder.Services.AddApplicationServices(builder.Configuration);

    // Registrar servicio de bloqueo de cuentas (OWASP A07:2021 - Identification and Authentication Failures)
    builder.Services.AddSingleton<IAccountLockoutService, AccountLockoutService>();

    // Configurar mensajería con MassTransit + RabbitMQ
    builder.Services.AddRabbitMQMessaging(builder.Configuration);

    // Configurar autenticación JWT
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured"))),
            ClockSkew = TimeSpan.Zero // Eliminar tolerancia de expiración
        };
    });

    // Configurar Rate Limiting (protección contra ataques de fuerza bruta)
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        // Rate limit específico para endpoints de autenticación
        options.AddPolicy("auth", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:Auth:PermitLimit", 5),
                    Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:Auth:WindowSeconds", 60)),
                    QueueLimit = builder.Configuration.GetValue<int>("RateLimiting:Auth:QueueLimit", 2),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                }));

        // Rate limit general para otros endpoints
        options.AddPolicy("general", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:General:PermitLimit", 100),
                    Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:General:WindowSeconds", 60))
                }));
    });

    // Configurar Controllers
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });

    // Configurar ProblemDetails para errores
    builder.Services.AddProblemDetails();

    // Configurar el Global Exception Handler
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    // Configurar Swagger/OpenAPI con soporte para JWT y Tags por grupo
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // ═══════════════════════════════════════════════════════════════
        // 📖 INFORMACIÓN DE LA API
        // ═══════════════════════════════════════════════════════════════
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "BookingSystem API",
            Description = "API para gestión de reservas con autenticación JWT Bearer",
            Contact = new OpenApiContact
            {
                Name = "Soporte",
                Email = "soporte@bookingsystem.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

        // ═══════════════════════════════════════════════════════════════
        // 🔐 CONFIGURACIÓN JWT BEARER (OpenAPI 3.0 - Mejor práctica)
        // ═══════════════════════════════════════════════════════════════
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Ingresa el token JWT. Ejemplo: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // ═══════════════════════════════════════════════════════════════
        // 🏷️ AGRUPACIÓN DE TAGS POR CONTROLADOR
        // ═══════════════════════════════════════════════════════════════
        options.TagActionsBy(apiDesc =>
        {
            // Agrupa por nombre del controlador
            if (apiDesc.ActionDescriptor.RouteValues.TryGetValue("controller", out var controller))
            {
                return new[] { controller };
            }
            
            // Para Minimal APIs, usar el primer segmento de la ruta
            return apiDesc.RelativePath?.Split('/').FirstOrDefault() is string path
                ? new[] { char.ToUpper(path[0]) + path[1..] }
                : new[] { "General" };
        });

        // 📑 Ordenar acciones por tag y ruta
        options.OrderActionsBy(apiDesc =>
            $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");

        // ═══════════════════════════════════════════════════════════════
        // 📖 COMENTARIOS XML PARA DOCUMENTACIÓN
        // ═══════════════════════════════════════════════════════════════
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }

        // Habilitar anotaciones de Swagger (para [SwaggerOperation])
        options.EnableAnnotations();
    });

    // Configurar CORS con orígenes permitidos desde configuración
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
        ?? new[] { "http://localhost:3000" };

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });

        // Política permisiva solo para desarrollo
        if (builder.Environment.IsDevelopment())
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        }
    });

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>("database")
        .AddRabbitMQHealthCheck(builder.Configuration);

    var app = builder.Build();

    // ========================================
    // CONFIGURACIÓN DEL PIPELINE HTTP
    // ========================================

    // Usar Serilog para logging de requests
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    // Middleware de correlación de requests
    app.UseMiddleware<CorrelationIdMiddleware>();

    // ================================================
    // MIDDLEWARES DE SEGURIDAD (OWASP)
    // ================================================
    
    // Headers de seguridad HTTP (OWASP)
    app.UseSecurityHeaders();
    
    // Validación y sanitización de requests (OWASP A03:2021 - Injection)
    app.UseRequestValidation();

    // Global Exception Handler
    app.UseExceptionHandler();

    // Swagger solo en Development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingSystem API v1");
            options.RoutePrefix = string.Empty;
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            options.DefaultModelsExpandDepth(-1); // Ocultar schemas por defecto
            options.DisplayRequestDuration();
            options.EnablePersistAuthorization(); // Persistir token JWT
        });
    }

    app.UseHttpsRedirection();
    
    // Rate Limiting
    app.UseRateLimiter();

    // CORS - usar política restrictiva en producción
    if (app.Environment.IsDevelopment())
    {
        app.UseCors("AllowAll");
    }
    else
    {
        app.UseCors("AllowSpecificOrigins");
    }

    app.UseAuthentication();
    app.UseAuthorization();

    // Aplicar migraciones automáticamente (crear tablas si no existen)
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        // Solo si no es InMemory database
        if (!string.IsNullOrEmpty(connectionString) && !connectionString.Contains("YOUR_SERVER"))
        {
            try
            {
                Log.Information("Verificando/Creando base de datos y tablas...");
                // Usar EnsureCreated para crear las tablas basadas en el modelo
                var created = db.Database.EnsureCreated();
                if (created)
                {
                    Log.Information("Base de datos y tablas creadas correctamente");
                }
                else
                {
                    Log.Information("Base de datos ya existe");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al crear/verificar la base de datos");
                throw;
            }
        }
    }

    // Health check endpoint
    app.MapHealthChecks("/health");

    app.MapControllers();

    Log.Information("BookingSystemAPI iniciada correctamente en {Environment}", app.Environment.EnvironmentName);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Clase parcial para acceso desde tests de integración
/// </summary>
public partial class Program { }
