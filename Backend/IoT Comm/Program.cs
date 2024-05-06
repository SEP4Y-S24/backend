using IoT_Comm;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

host.Run();