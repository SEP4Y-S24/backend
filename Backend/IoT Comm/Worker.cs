using EfcDatabase.Context;
using Services.IServices;
using Services.Services;

namespace IoT_Comm;

public class Worker : BackgroundService
{
    // private readonly ILogger<Worker> _logger;
    private readonly IClockService _clockService;

    public Worker()
    {
        // _logger = logger;
        // _clockService = clockService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var server= new TcpServer();
        while (!stoppingToken.IsCancellationRequested)
        {
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}