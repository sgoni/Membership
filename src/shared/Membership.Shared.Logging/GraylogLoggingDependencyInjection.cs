using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Membership.Shared.Logging;

public static class GraylogLoggingDependencyInjection
{
    public static IServiceCollection AddGraylogLogging(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Cargar la configuración de Graylog desde appsettings.json
        services.Configure<GraylogConfig>(configuration.GetSection("GraylogConfig"));

        // Registrar la clase GraylogLogger como un servicio
        services.AddSingleton<IServiceLogger, GraylogLogger>();

        return services;
    }
}