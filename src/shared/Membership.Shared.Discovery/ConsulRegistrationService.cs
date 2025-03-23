using Microsoft.Extensions.Hosting;

namespace Membership.Shared.Discovery;

public class ConsulRegistrationService : BackgroundService
{
    private readonly ConsulServiceDiscovery _consulServiceDiscovery;

    public ConsulRegistrationService(ConsulServiceDiscovery consulServiceDiscovery)
    {
        _consulServiceDiscovery = consulServiceDiscovery;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consulServiceDiscovery.RegisterServiceAsync(stoppingToken);
        // Aquí puedes añadir lógica adicional si necesitas hacer ping o renovación periódica en Consul
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Puedes agregar aquí cualquier limpieza antes de detener el servicio
        await _consulServiceDiscovery
            .DesregisterServiceAsync(cancellationToken); // Este método podría desregistrar el servicio
        await base.StopAsync(cancellationToken);
    }
}