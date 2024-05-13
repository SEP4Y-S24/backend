using EfcDatabase.Context;
using Services.IServices;
using Services.Services;

namespace IoT_Comm;

public class Worker : BackgroundService
{
    // private readonly ILogger<Worker> _logger;

    public Worker()
    {
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