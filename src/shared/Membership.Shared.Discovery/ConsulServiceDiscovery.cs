using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Membership.Shared.Discovery;

public interface IServiceDiscovery
{
    Task RegisterServiceAsync(CancellationToken cancellationToken);

    Task DesregisterServiceAsync(CancellationToken cancellationToken);
}

public class ConsulServiceDiscovery : IServiceDiscovery
{
    private readonly ConsulConfig _config;
    private readonly IConsulClient _consulClient;
    private readonly ILogger<ConsulRegistrationService> _logger;

    public ConsulServiceDiscovery(IOptions<ConsulConfig> options)
    {
        _config = options.Value;
        _consulClient = new ConsulClient(config => { config.Address = new Uri(_config.ConsulHost); });
    }

    public async Task RegisterServiceAsync(CancellationToken cancellationToken)
    {
        //var httpCheck = new AgentServiceCheck()
        //{
        //    HTTP = _config.HealthCheckUrl,
        //    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
        //    Interval = TimeSpan.FromSeconds(30),
        //};

        var registration = new AgentServiceRegistration
        {
            ID = _config.ServiceId,
            Name = _config.ServiceName,
            Address = _config.ServiceAddress,
            Port = _config.ServicePort
            //Checks = new[] { httpCheck }
        };

        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        Console.WriteLine($"Service {_config.ServiceName} registered in Consul");

        var services = _consulClient.Agent.Services().Result.Response;
        foreach (var service in services)
        {
            var checks = _consulClient.Health
                .Checks(_config.ServiceName)
                .Result;

            foreach (var checkResult in checks.Response)
                Console.WriteLine($"{checkResult.ServiceID} - {checkResult.Status.Status}");
        }

        // Deregistrar el servicio cuando la aplicación se apague
        cancellationToken.Register(() =>
        {
            _consulClient.Agent.ServiceDeregister(_config.ServiceId).Wait();
            Console.WriteLine($"Service {_config.ServiceName} deregistered from Consul");
        });
    }

    public async Task DesregisterServiceAsync(CancellationToken cancellationToken)
    {
        _consulClient.Agent.ServiceDeregister(_config.ServiceId).Wait();
        Console.WriteLine($"Service {_config.ServiceName} deregistered from Consul");
    }
}