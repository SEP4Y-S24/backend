using Application.DAO;
using EfcDatabase.Context;
using IoT_Comm;
using Services.IServices;
using Services.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ClockContext>();
        services.AddScoped<IClockDAO, ClockDAO>();
        services.AddScoped<IClockService, ClockService>();
        services.AddHostedService<Worker>();
    })
    .Build();


host.Run();