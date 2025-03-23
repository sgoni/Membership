namespace Membership.Shared.Logging;

public class GraylogConfig
{
    public string Host { get; set; } = "localhost"; // Graylog Host 
    public int Port { get; set; } = 12201; // GELF port (UDP by default)
    public string ApplicationName { get; set; } = "MyApp"; // Application name
    public bool UseUdp { get; set; } = true; // Use UDP or TCP
    public bool Enabled { get; set; } = false;
}