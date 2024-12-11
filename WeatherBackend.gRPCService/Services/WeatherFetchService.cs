namespace WeatherBackend.gRPCService.Services;

public class WeatherFetchService(ILogger<WeatherFetchService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("DataFetchService is running.");

            try
            {
                await FetchDataAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while fetching data.");
            }

            // execute every 5 min
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
    
    private Task FetchDataAsync()
    {
        // Your data fetching logic here
        logger.LogInformation("Fetching data...");
        return Task.CompletedTask;
    }
}