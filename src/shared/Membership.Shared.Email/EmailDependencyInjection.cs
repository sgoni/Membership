using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Membership.Shared.Email;

public static class EmailDependencyInjection
{
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Cargar la configuración de Email desde appsettings.json
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // Registrar la clase ConsulServiceRegistrar como un servicio
        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }
}