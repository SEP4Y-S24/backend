using AlarmServices.Context;
using AlarmServices.DAOImplementation;
using AlarmServices.IDAO;
using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace N_Unit_Services;

public class AlarmServicesTest
{
    private IAlarmDAO _alarmDao;
    private ClockContext _context;
    private IConfiguration _configuration;
    
    [SetUp]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\nural\\RiderProjects\\backend\\Backend\\AlarmServices\\appsettings.json",
                optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();

        // Ensure _context is initialized properly
        _context = CreateTestContext();
        if (_context == null)
        {
            throw new Exception("_context is null");
        }

        // Ensure _todoDAO is initialized properly
        _alarmDao = new AlarmDAO(_context);
        if (_alarmDao == null)
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
    public async Task CreateAsync_ValidAlarm()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        
        // Arrange
        using (var context = CreateTestContext())
        {
            var dao = new AlarmDAO(_context);
        }

        var alarm = new Alarm
        {
            Id = Guid.NewGuid(),
            ClockId = Guid.NewGuid(),
            SetOffTime = utcNow.AddDays(2),
            IsActive = false
        };
        var result = await _alarmDao.CreateAsync(alarm);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(alarm.Id, result.Id);
        Assert.AreEqual(alarm.ClockId, result.ClockId);
        Assert.AreEqual(alarm.SetOffTime, result.SetOffTime);
        
        // Verify that the alarm was added to the database
        var addedAlarm = await _context.Alarms.FindAsync(alarm.Id);
        Assert.IsNotNull(addedAlarm);
        Assert.AreEqual(alarm.Id, addedAlarm.Id);
        Assert.AreEqual(alarm.ClockId, addedAlarm.ClockId);
    }
    
    [Test]
    public async Task GetByIdAsync_ExistingAlarmId_ShouldReturnAlarm()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var alarm = new Alarm
        {
            Id = Guid.NewGuid(),
            ClockId = Guid.NewGuid(),
            SetOffTime = utcNow.AddDays(2),
            IsActive = false
        };

        await _context.Alarms.AddAsync(alarm);
        await _context.SaveChangesAsync();

        // Act
        var retrievedAlarm = await _alarmDao.GetByIdAsync(alarm.Id);

        // Assert
        Assert.IsNotNull(retrievedAlarm);
        Assert.AreEqual(alarm.Id, retrievedAlarm.Id);
        Assert.AreEqual(alarm.ClockId, retrievedAlarm.ClockId);
        Assert.AreEqual(alarm.IsActive, retrievedAlarm.IsActive);
    }
    
    [Test]
    public async Task DeleteAsync_ExistingTodoId_ShouldDeleteTodo()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var alarm = new Alarm
        {
            Id = Guid.NewGuid(),
            ClockId = Guid.NewGuid(),
            SetOffTime = utcNow.AddDays(2),
            IsActive = false
        };

        await _context.Alarms.AddAsync(alarm);
        await _context.SaveChangesAsync();

        // Act
        await _alarmDao.DeleteAsync(alarm.Id);

        // Assert
        var deletedAlarm = await _context.Alarms.FindAsync(alarm.Id);
        Assert.IsNull(deletedAlarm);
    }
    
    [Test]
    public async Task UpdateAsync_ValidAlarm_ShouldUpdateTodo()
    {
        var localNow = DateTime.Now;
        var utcNow = localNow.ToUniversalTime();
        // Arrange
        
        var originalAlarm = new Alarm
        {
            Id = Guid.NewGuid(),
            ClockId = Guid.NewGuid(),
            SetOffTime = utcNow.AddDays(2),
            IsActive = false
        };

        // Add the original alarm to the database
        await _context.Alarms.AddAsync(originalAlarm);
        await _context.SaveChangesAsync();

        // Modify the alarm
        var updatedAlarm = new Alarm
        {
            Id = originalAlarm.Id,
            ClockId = originalAlarm.ClockId,
            SetOffTime = utcNow.AddDays(4),
            IsActive = true,
        };

        // Act
        await _alarmDao.UpdateAsync(updatedAlarm);

        // Assert
        var retrievedAlarm = await _context.Alarms.FindAsync(originalAlarm.Id);
        Assert.IsNotNull(retrievedAlarm);
        // Assert.AreEqual(updatedAlarm.Id, retrievedAlarm.Id);
        Assert.AreEqual(updatedAlarm.ClockId, retrievedAlarm.ClockId); 
        Assert.AreEqual(updatedAlarm.IsActive, retrievedAlarm.IsActive);
        Assert.AreEqual(updatedAlarm.SetOffTime, retrievedAlarm.SetOffTime);
    }
}