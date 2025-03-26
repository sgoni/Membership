namespace Membership.Shared.Discovery;

public class ConsulConfig
{
    public string? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceAddress { get; set; }
    public int ServicePort { get; set; }
    public string? HealthCheckUrl { get; set; }
    public string? ConsulHost { get; set; }
}