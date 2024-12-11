using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace WeatherBackend.Infrastructure.Extensions;

public static class SerilogServiceExtensions
{
    public static void AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var logLevel = configuration["LOG_LEVEL"] ?? "Information";
        var logEventLevel = Enum.Parse<LogEventLevel>(logLevel, true);
        var logName = configuration["LOG_NAME"] ?? "WeatherBackend.log";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .WriteTo.File(
                Path.Combine(AppContext.BaseDirectory, logName), 
                rollingInterval: RollingInterval.Month,
                buffered: false)
            .WriteTo.Console()
            .CreateLogger();

        // Register Serilog
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }
}