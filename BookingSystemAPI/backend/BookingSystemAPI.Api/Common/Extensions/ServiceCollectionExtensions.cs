using BookingSystemAPI.Api.Common.Options;
using BookingSystemAPI.Api.Repositories;
using BookingSystemAPI.Api.Services;

namespace BookingSystemAPI.Api;

/// <summary>
/// Extensiones para configuración de servicios de la aplicación.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra los servicios de la aplicación en el contenedor de DI.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>Colección de servicios con los servicios registrados.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar opciones de configuración
        services.Configure<QueryOptions>(
            configuration.GetSection(QueryOptions.SectionName));

        // Registrar repositorios genéricos
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Registrar repositorios específicos
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Registrar servicios
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoomReportService, RoomReportService>();

        return services;
    }
}
