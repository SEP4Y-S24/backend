﻿
using Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.DAOImplementation;
using Shared.IDAO;
using Shared.IService;
using Shared.Service;
using TodoServices.DAOImplementation;

namespace Shared;

public static class ServiceFactory
{
    private static ClockContext context = null;
    private static IUserDAO _userDao = null;
    private static IClockDAO _clockDao = null;
    private static IAlarmDAO _alarmDao = null;
    private static IAlarmService _alarmService = null;
    private static ITodoService _todoService = null;
    private static ITodoDAO _todoDao = null;
    private static ITagDao _tagDao = null;
    public static ClockContext _todoContext = null;
    public static TagService _tagService = null;

    public static ClockContext GetContext()
    {
        if (context == null)
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

            context = new ClockContext(options);

        }
        return context;
    }
    
    public static IUserDAO GetUserDao()
    {
        if (_userDao == null)
            _userDao = new UserDAO(GetContext());

        return _userDao;
    }
    public static IAlarmDAO GetAlarmDAO()
    {
        if (_alarmDao == null)
            _alarmDao = new AlarmDAO(GetContext());

        return _alarmDao;
    }
    public static IClockDAO GetClockDAO()
    {
        if (_clockDao == null)
            _clockDao = new ClockDAO(GetContext());

        return _clockDao;
    }
    public static IAlarmService GetAlarmService()
    {
        if (_alarmService == null)
            _alarmService = new AlarmService(GetAlarmDAO());

        return _alarmService;
    }

    public static ITodoService GetTodoService()
    {
        if (_todoService==null)
        {
            _todoService = new TodoService(GetTodoDAO(),GetUserDao(),GetTagDAO());
        }

        return _todoService;
    }

    public static ITagService GetTagService()
    {
        if (_tagService ==null)
        {
            _tagService = new TagService(GetTodoDAO(), GetTagDAO());
        }
        return _tagService;
    }

    public static ITagDao GetTagDAO()
    {
        if (_tagDao ==null)
        {
            _tagDao = new TagDao(GetContext());
        }
        return _tagDao;
    }

    public static ITodoDAO GetTodoDAO()
    {
        if (_todoDao==null)
        {
            _todoDao = new TodoDao(GetContext());
        }
        return _todoDao;
    }
}