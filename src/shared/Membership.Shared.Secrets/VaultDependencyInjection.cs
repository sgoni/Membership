using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Membership.Shared.Secrets;

public static class VaultDependencyInjection
{
    public static IServiceCollection AddVaultService(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Cargar la configuración de Vault desde appsettings.json
        services.Configure<VaultProviderSettings>(configuration.GetSection("VaultConfig"));

        // Registrar la clase VaultConfigurationProvider como un servicio
        services.AddScoped<ISecretManager, VaultConfigurationProvider>();

        return services;
    }
}