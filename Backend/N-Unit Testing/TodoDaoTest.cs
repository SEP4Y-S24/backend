using EfcDatabase.Context;
using EfcDatabase.DAOImplementation;
using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace N_Unit_Testing;

public class TodoDaoTest
{
    public class ToDoDaoTest
    {
        private IToDoDAO _todoDAO;
        private ClockContext _context;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            // Load configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("C:\\Users\\nural\\RiderProjects\\backend\\Backend\\WebApi\\appsettings.json",
                    optional: false, reloadOnChange: true);
            _configuration = configurationBuilder.Build();

            // Ensure _context is initialized properly
            _context = CreateTestContext();
            if (_context == null)
            {
                throw new Exception("_context is null");
            }

            // Ensure _todoDAO is initialized properly
            _todoDAO = new ToDoDAO(_context);
            if (_todoDAO == null)
            {
                throw new Exception("_todoDAO is null");
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
        public async Task CreateAsync_ValidTodo_ReturnsAddedTodo()
        {
            // Arrange
            using (var context = CreateTestContext())
            {
                var dao = new ToDoDAO(context);
                var user = new User
                {
                    Id = Guid.NewGuid(),
                };

                // Convert local time to UTC
                var localNow = DateTime.Now;
                var utcNow = localNow.ToUniversalTime();

                var todo = new ToDo
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    UserId = user.Id,
                    Name = "Test ToDo",
                    Description = "Test description",
                    Deadline = utcNow.AddDays(1), // Use UTC time
                    Status = Status.Started
                };

                // Act
                var result = await dao.CreateAsync(todo);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(todo.Id, result.Id);
                Assert.AreEqual(todo.Name, result.Name);
                Assert.AreEqual(todo.Description, result.Description);

                // Verify that the todo was added to the database
                var addedTodo = await context.Todos.FindAsync(todo.Id);
                Assert.IsNotNull(addedTodo);
                Assert.AreEqual(todo.Name, addedTodo.Name);
                Assert.AreEqual(todo.UserId, addedTodo.UserId);
            }
        }
        [Test]
        public async Task UpdateAsync_ValidTodo_ShouldUpdateTodo()
        {
            var localNow = DateTime.Now;
            var utcNow = localNow.ToUniversalTime();
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            var originalTodo = new ToDo
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
                Name = "Original ToDo",
                Description = "Original description", // Original description
                Deadline = utcNow.AddDays(1),
                Status = Status.Started
            };

            // Add the original todo to the database
            await _context.Todos.AddAsync(originalTodo);
            await _context.SaveChangesAsync();

            // Modify the todo
            var updatedTodo = new ToDo
            {
                Id = originalTodo.Id,
                User = user,
                UserId = user.Id,
                Name = "Updated ToDo",
                Description = "Updated description", // Updated description
                Deadline = utcNow.AddDays(2), // Modify the deadline
                Status = Status.Finished // Change the status
            };

            // Act
            await _todoDAO.UpdateAsync(updatedTodo);

            // Assert
            var retrievedTodo = await _context.Todos.FindAsync(originalTodo.Id);
            Assert.IsNotNull(retrievedTodo);
            Assert.AreEqual(updatedTodo.Name, retrievedTodo.Name);
            Assert.AreEqual(updatedTodo.Description, retrievedTodo.Description); // Compare descriptions
            Assert.AreEqual(updatedTodo.Deadline, retrievedTodo.Deadline);
            Assert.AreEqual(updatedTodo.Status, retrievedTodo.Status);
        }

    }
}