using ClockServices.Context;
using ClockServices.DAOImplementation;
using Microsoft.Extensions.Configuration;
using ClockServices.IDAO;
using ClockServices.Model;
using Microsoft.EntityFrameworkCore;

namespace N_Unit_Services;

public class ClockServiceTest
{
    private IClockDAO _clockDao;
    private ClockContext _context;
    private IConfiguration _configuration;
    [SetUp]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\denim\\Documents\\VIA\\4-Semester\\SEP4\\backend\\Backend\\ClockServices\\appsettings.json",
                optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();

        // Ensure _context is initialized properly
        _context = CreateTestContext();
        if (_context == null)
        {
            throw new Exception("_context is null");
        }

        // Ensure _todoDAO is initialized properly
        _clockDao = new ClockDAO(_context);
        if (_clockDao == null)
        {
            throw new Exception("_alarmDAO is null");
        }
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
    public async Task CreateAsync_ValidClock()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        
        // Arrange
        using (var context = CreateTestContext())
        {
            var clock_dao = new ClockDAO(_context);
        }
        var user_dao = new UserDAO(_context);

        var user = new User()
        {
            Id = new Guid()
        };
        user_dao.CreateAsync(user);
        var messages = new List<Message>();
        var clock = new Clock()
        {
            Id = Guid.NewGuid(),
            Name = "Test Clock",
            TimeOffset = 4,
            OwnerId = user.Id,
            Owner = user,
            Messages = messages
        };
        var result = await _clockDao.CreateAsync(clock);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(clock.Id, result.Id);
        Assert.AreEqual(clock.TimeOffset, result.TimeOffset);
        Assert.AreEqual(clock.Name, result.Name);
        Assert.AreEqual(clock.Messages, result.Messages);
        Assert.AreEqual(clock.Owner, result.Owner);
        
        // Verify that the alarm was added to the database
        var addedClock = await _context.Clocks.FindAsync(clock.Id);
        Assert.IsNotNull(addedClock);
        Assert.AreEqual(clock.Id, addedClock.Id);
        Assert.AreEqual(clock.TimeOffset, addedClock.TimeOffset);
        Assert.AreEqual(clock.Name, addedClock.Name);
        Assert.AreEqual(clock.Messages, addedClock.Messages);
        Assert.AreEqual(clock.Owner, addedClock.Owner);
    }
    [Test]
    public async Task UpdateAsync_ValidTodo_ShouldUpdateTodo()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        // Arrange
        var user_dao = new UserDAO(_context);

        var user = new User { Id = Guid.NewGuid() };
        await user_dao.CreateAsync(user);

        var messages = new List<Message>();
        var originalClock = new Clock()
        {
            Id = Guid.NewGuid(),
            Owner = user,
            OwnerId = user.Id,
            Name = "Original Clock",
            TimeOffset = 4,
            Messages = messages
        };

        // Add the original todo to the database
        var result = await _clockDao.CreateAsync(originalClock);
        var newUser = new User()
        {
            Id = Guid.NewGuid()
        };
        await user_dao.CreateAsync(newUser);

        // Modify the todo
        var updatedTodo = new Clock()
        {
            Id = originalClock.Id,
            Name = "Updated ToDo",
            TimeOffset = 5,
            Messages = messages,
            Owner = user,
            OwnerId = user.Id
        };

        // Act
        await _clockDao.UpdateAsync(updatedTodo);

        // Assert
        var retrievedClock = await _context.Clocks.FindAsync(originalClock.Id);
        Assert.IsNotNull(retrievedClock);
        Assert.AreEqual(updatedTodo.Name, retrievedClock.Name);
        Assert.AreEqual(updatedTodo.TimeOffset, retrievedClock.TimeOffset); // Compare descriptions
    }
    // Unit test for GetAllAsync method
    [Test]
    public async Task GetAllAsync_ShouldReturnAllTodos()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var messages = new List<Message>();

        var clock = new Clock
        {
            Id = Guid.NewGuid(),
            Owner = user,
            OwnerId = user.Id,
            Name = "Test Clock 1",
            TimeOffset = 4,
            Messages = messages
        };
        var clock2 = new Clock()
        {
            Id = Guid.NewGuid(),
            Owner = user,
            OwnerId = user.Id,
            Name = "Test Clock 2",
            TimeOffset = 4,
            Messages = messages
        };

        await _context.Clocks.AddRangeAsync(clock, clock2);
        await _context.SaveChangesAsync();

        // Act
        var clocks = await _clockDao.GetAll();

        // Assert
        Assert.IsNotNull(clocks);
        Assert.AreEqual(21, clocks.Count());
    }
}