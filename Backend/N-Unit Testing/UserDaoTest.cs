using EfcDatabase.Context;
using EfcDatabase.DAOImplementation;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace N_Unit_Testing;

public class UserDaoTest
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
    public async Task CreateAsync_ValidUser_ReturnsAddedUser()
    {
        // Arrange
        using (var context = CreateTestContext())
        {
            var dao = new UserDAO(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                MessagesRecieved = new List<Message>(),
                MessagesSent = new List<Message>()
            };


            // Act
            var result = await dao.CreateAsync(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(user.Id));

            // Assert.AreEqual(clock.Name, result.Name);
            // Assert.AreEqual(clock.TimeZone, result.TimeZone);

            // Verify that the clock was added to the database
            var addedUser = await context.Users.FindAsync(user.Id);
            Assert.IsNotNull(addedUser);
            Assert.That(addedUser.Id, Is.EqualTo(user.Id));

            // Assert.AreEqual(clock.TimeZone, addedClock.TimeZone);
        }
    }
}