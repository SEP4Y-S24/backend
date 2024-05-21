
using AClockService.Context;
using AClockService.DAOImplementation;
using AClockService.IDAO;
using AClockService.IServices;
using AClockService.Model;
using AClockService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AClockService;

public static class ServiceFactory
{
    private static ClockContext _clockContext = null;
    private static IUserDAO _userDao = null;
    private static IClockDAO _clockDao = null;
    private static IMessageDao _messageDao = null;
    private static IClockService _clockService = null;

    public static ClockContext GetClockontext()
    {
        if (_clockContext == null)
        {
            // Load configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("./host.json",
                    optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            // Ensure _context is initialized properly
            var connectionString = configuration.GetConnectionString("Database");
            var options = new DbContextOptionsBuilder<ClockContext>()
                .UseNpgsql(connectionString)
                .Options;

            _clockContext = new ClockContext(options);

        }
        return _clockContext;
    }
    public static IUserDAO GetUserDao()
    {
        if (_userDao == null)
            _userDao = new UserDAO(GetClockontext());

        return _userDao;
    }
    public static IMessageDao GetMessageDAO()
    {
        if (_messageDao == null)
            _messageDao = new MessageDAO(GetClockontext());

        return _messageDao;
    }
    public static IClockDAO GetClockDAO()
    {
        if (_clockDao == null)
            _clockDao = new ClockDAO(GetClockontext());

        return _clockDao;
    }
    public static IClockService GetClockService()
    {
        if (_clockService == null)
            _clockService = new ClockService(GetClockDAO(), GetUserDao());

        return _clockService;
    }
    
}