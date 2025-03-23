using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Membership.Shared.Logging;

public interface IServiceLogger
{
    void LogError(string message, Exception ex);
    void LogInformation(string message);
}

public class GraylogLogger : IServiceLogger, IDisposable, IAsyncDisposable
{
    private readonly Logger _logger;

    public GraylogLogger(IOptions<GraylogConfig> options)
    {
        var config = options.Value;

        var graylogSinkConfig = new GraylogSinkOptions
        {
            HostnameOrAddress = config.Host,
            Port = config.Port,
            TransportType = config.UseUdp ? TransportType.Udp : TransportType.Http,
            Facility = config.ApplicationName
        };

        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Graylog(graylogSinkConfig)
            .CreateLogger();
    }

    public async ValueTask DisposeAsync()
    {
        await _logger.DisposeAsync();
    }

    public void Dispose()
    {
        _logger.Dispose();
    }

    public void LogError(string message, Exception ex)
    {
        _logger.Error(ex, message);
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }
}