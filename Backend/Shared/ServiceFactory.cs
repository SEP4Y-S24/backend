
using Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.DAOImplementation;
using Shared.Helpers;
using Shared.IDAO;
using Shared.IService;
using Shared.Service;
using TodoServices.DAOImplementation;

namespace Shared;

public static class ServiceFactory
{
    private static ClockContext context = null;
    private static IUserDAO _userDao = null;
    private static IMessageDao _messageDao = null;
    private static IClockDAO _clockDao = null;
    private static IAlarmDAO _alarmDao = null;
    private static ITodoDAO _todoDao = null;
    private static ITagDao _tagDao = null;
    private static IContactDAO _contactDao = null;
    private static IEventDAO _eventDao = null;
    private static IMeasurementDAO _measurementDao = null;
    private static IAlarmService _alarmService = null;
    private static IMessageService _messageService = null;
    private static IUserService _userService = null;
    private static IClockService _clockService = null;
    private static ITodoService _todoService = null;
    private static IContactService _contactService = null;
    public static ITagService TagService = null;
    public static IEventService _eventService = null;
    public static IMeasurementService _measurementService = null;
    public static IJwtUtils _JwtUtils = null;

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
    public static IContactDAO GetContactDao()
    {
        if (_contactDao == null)
            _contactDao = new ContactDAO(GetContext());

        return _contactDao;
    }
    public static IMeasurementDAO GetMeasurementDao()
    {
        if (_measurementDao == null)
            _measurementDao = new MeasurementDAO(GetContext());

        return _measurementDao;
    }
    public static IEventDAO GetEventDao()
    {
        if (_eventDao == null)
            _eventDao = new EventDAO(GetContext());

        return _eventDao;
    }

    public static IJwtUtils GetJwtUtils()
    {
        if (_JwtUtils == null)
            _JwtUtils = new JwtUtils();

        return _JwtUtils;
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
    public static IMessageDao GetMessageDao()
    {
        if (_messageDao == null)
            _messageDao = new MessageDAO(GetContext());

        return _messageDao;
    }
    public static IUserService GetUserService()
    {
        if (_userService == null)
            _userService = new UserService(GetUserDao(), GetClockDAO(), GetTodoDAO());

        return _userService;
    }
    public static IMeasurementService GetMeasurementService()
    {
        if (_measurementService == null)
            _measurementService = new MeasurementService(GetClockDAO(), GetMeasurementDao());

        return _measurementService;
    }

    public static IContactService GetContactService()
    {
        if (_contactService == null)
            _contactService = new ContactService(GetContactDao(), GetUserDao());

        return _contactService;
    }

    public static IEventService GetEventService()
        {
            if (_eventService == null)
                _eventService = new EventService(GetEventDao(), GetUserDao(), GetTagDAO());

            return _eventService;
        }
        public static IMessageService GetMessageService()
        {
            if (_messageService == null)
                _messageService = new MessageService(GetMessageDao());

            return _messageService;
        }
        public static IClockService GetClockService()
        {
            if (_clockService == null)
                _clockService = new ClockService(GetClockDAO(), GetUserDao());

            return _clockService;
        }
        public static IAlarmService GetAlarmService()
        {
            if (_alarmService == null)
                _alarmService = new AlarmService(GetAlarmDAO());

            return _alarmService;
        }

        public static ITodoService GetTodoService()
        {
            if (_todoService == null)
            {
                _todoService = new TodoService(GetTodoDAO(), GetUserDao(), GetTagDAO());
            }

            return _todoService;
        }

        public static ITagService GetTagService()
        {
            if (TagService == null)
            {
                TagService = new TagService(GetTodoDAO(), GetTagDAO());
            }
            return TagService;
        }

        public static ITagDao GetTagDAO()
        {
            if (_tagDao == null)
            {
                _tagDao = new TagDao(GetContext());
            }
            return _tagDao;
        }

        public static ITodoDAO GetTodoDAO()
        {
            if (_todoDao == null)
            {
                _todoDao = new TodoDao(GetContext());
            }
            return _todoDao;
        }
    }