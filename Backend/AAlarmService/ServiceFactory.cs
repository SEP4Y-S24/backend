
using AAlarmService.DAOImplementation;
using AAlarmService.IDAO;
using AAlarmService.IService;
using AAlarmService.Service;
using AAlarmService.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AAlarmService;

public static class ServiceFactory
{
    private static AlarmContext _alarmContext = null;
    private static IUserDAO _userDao = null;
    private static IClockDAO _clockDao = null;
    private static IAlarmDAO _alarmDao = null;
    private static IAlarmService _alarmService = null;

    public static AlarmContext GetAlarmContext()
    {
        if (_alarmContext == null)
        {
            // Load configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("./host.json",
                    optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            // Ensure _context is initialized properly
            var connectionString = configuration.GetConnectionString("Database");
            var options = new DbContextOptionsBuilder<AlarmContext>()
                .UseNpgsql(connectionString)
                .Options;

            _alarmContext = new AlarmContext(options);

        }
        return _alarmContext;
    }
    public static IUserDAO GetUserDao()
    {
        if (_userDao == null)
            _userDao = new UserDAO(GetAlarmContext());

        return _userDao;
    }
    public static IAlarmDAO GetAlarmDAO()
    {
        if (_alarmDao == null)
            _alarmDao = new AlarmDAO(GetAlarmContext());

        return _alarmDao;
    }
    public static IClockDAO GetClockDAO()
    {
        if (_clockDao == null)
            _clockDao = new ClockDAO(GetAlarmContext());

        return _clockDao;
    }
    public static IAlarmService GetAlarmService()
    {
        if (_alarmService == null)
            _alarmService = new AlarmService(GetAlarmDAO());

        return _alarmService;
    }
    
}