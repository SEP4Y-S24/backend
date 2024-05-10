using EfcDatabase.Context;
using EfcDatabase.DAOImplementation;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Configuration;

namespace N_Unit_Testing;

public class ClockDaoTest
{
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\nural\\RiderProjects\\backend\\Backend\\WebApi\\appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();
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
    public async Task CreateAsync_ValidClock_ReturnsAddedClock()
    {
        // Arrange
        using (var context = CreateTestContext())
        {
            var dao = new ClockDAO(context);
            var owner= new User
            {
                Id = Guid.NewGuid(),
            };
            var clock = new Clock
            {
                Owner = owner,
                OwnerId = owner.Id,
                Id = Guid.NewGuid(),
                Name = "Test Clock",
                TimeOffset = '0'
            };

            // Act
            var result = await dao.CreateAsync(clock);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(clock.Id, result.Id);
            Assert.AreEqual(clock.Name, result.Name);
            Assert.AreEqual(clock.TimeZone, result.TimeZone);

            // Verify that the clock was added to the database
            var addedClock = await context.Clocks.FindAsync(clock.Id);
            Assert.IsNotNull(addedClock);
            Assert.AreEqual(clock.Name, addedClock.Name);
            Assert.AreEqual(clock.TimeZone, addedClock.TimeZone);
        }
    }
}