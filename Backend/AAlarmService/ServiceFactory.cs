﻿
using AAlarmServices.DAOImplementation;
using AAlarmServices.IDAO;
using AAlarmServices.IService;
using AAlarmServices.Service;
using AlarmServices.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AAlarmService;

public static class ServiceFactory
{
    private static AlarmContext _alarmContext = null;
    private static IUserDAO _userDao = null;
    private static IClockDAO _clockDao = null;
    private static IMessageDAO _messageDao = null;
    private static IAlarmDAO _alarmDao = null;
    private static IAlarmService _alarmService = null;

    public static AlarmContext GetAlarmContext()
    {
        if (_alarmContext == null)
        {
            // Load configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("C:\\Users\\nural\\RiderProjects\\backend\\Backend\\TodoServices\\appsettings.json",
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
            _userDao = new UserDAO(_alarmContext);

        return _userDao;
    }
    public static IAlarmDAO GetAlarmDAO()
    {
        if (_alarmDao == null)
            _alarmDao = new AlarmDAO(_alarmContext);

        return _alarmDao;
    }
    public static IClockDAO GetClockDAO()
    {
        if (_clockDao == null)
            _clockDao = new ClockDAO(_alarmContext);

        return _clockDao;
    }
    public static IMessageDAO GetMessageDAO()
    {
        if (_messageDao == null)
            _messageDao = new MessageDAO(_alarmContext);

        return _messageDao;
    }
    public static IAlarmService GetAlarmService()
    {
        if (_alarmService == null)
            _alarmService = new AlarmService(_alarmDao);

        return _alarmService;
    }
    
}