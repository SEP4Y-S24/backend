using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Context;

namespace TCPServer;

public class ServiceFactory
{
    private static ClockContext context = null;
    public static ClockContext GetContext()
    {
        if (context == null)
        {
            // Load configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("D:\\My files\\Classes\\Sem 4\\SEP4\\backend\\TCPServer\\TCPServer\\host.json",
                    optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            // Ensure _context is initialized properly
            var connectionString = configuration.GetConnectionString("Database");
            var options = new DbContextOptionsBuilder<ClockContext>()
                .UseNpgsql(connectionString)
                .Options;

            context = new ClockContext(options);

        }
        return context;
    }
}