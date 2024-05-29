
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Shared.Context;
using Shared.DAOImplementation;
using Shared.IDAO;



namespace N_Unit_Services;

public class UserServicesTest
{
    private IUserDAO _userDao;
    private IMessageDao _messageDao;
    private ClockContext _context;
    private IConfiguration _configuration;
    [SetUp]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("./host.json",
                optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();

        // Ensure _context is initialized properly
        _context = CreateTestContext();
        if (_context == null)
        {
            throw new Exception("_context is null");
        }

        // Ensure _todoDAO is initialized properly
        _userDao = new UserDAO(_context);
        if (_userDao == null)
        {
            throw new Exception("_alarmDAO is null");
        }
        _messageDao = new MessageDAO(_context);

    }
    private ClockContext CreateTestContext()
    {
        // Retrieve the connection string from configuration
        var connectionString = _configuration.GetConnectionString("Database");
        var options = new DbContextOptionsBuilder<ClockContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new ClockContext(options);
    }
    [Test]
    public async Task CreateAsync_ValidUser()
    {
        // Arrange
        var user_dao = new UserDAO(_context);
        var message_dao = new MessageDAO(_context);

        var user = new User()
        {
            Id = new Guid(),
            Email = "ek@gmail.com",
            Name="Name",
            PasswordHash = "cdsjlh"
        };
        var messages = new List<Message>();
        var result = await _userDao.CreateAsync(user);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Id, result.Id);
        // Verify that the alarm was added to the database
        var addedUser = await _context.Users.FindAsync(user.Id);
        Assert.IsNotNull(addedUser);
        Assert.AreEqual(user.Id, addedUser.Id);
    }
    [Test]
    public async Task CreateAsync_ValidMessage()
    {
        // Arrange
        var user_dao = new UserDAO(_context);
        var clock_dao = new ClockDAO(_context);

        var user = new User()
        {
            Id = new Guid(),
            Email = "ek@gmail.com",
            Name="Name",
            PasswordHash = "cdsjlh"
        }; 
        await user_dao.CreateAsync(user);
        var messages = new List<Message>();
        var clock = new Clock()
        {
            Id = Guid.NewGuid(),
            Name = "Test Clock",
            OwnerId = user.Id,
            Owner = user,
            Messages = messages
        };
        await clock_dao.CreateAsync(clock);
        Message message = new Message()
        {
            Reciever = user,
            Sender = user,
            Clock = clock,
            ClockId = clock.Id,
            ReceiverId = user.Id,
            SenderId = user.Id,
            Body = "New Message"
        };
        var result = await _messageDao.CreateAsync(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(message.Id, result.Id);
        Assert.AreEqual(message.Body, result.Body);
        // Assert
        var retrievedMessage = await _context.Messages.FindAsync(message.Id);
        Assert.IsNotNull(retrievedMessage);
        Assert.AreEqual(message.Body, retrievedMessage.Body);
    }
}