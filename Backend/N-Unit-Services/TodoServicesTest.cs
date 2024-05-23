
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Shared.Context;
using Shared.DAOImplementation;
using Shared.IDAO;

namespace N_Unit_Services;

public class TodoServicesTest
{
    private ITodoDAO _todoDao;
    private ClockContext _context;
    private IConfiguration _configuration;
    
    [SetUp]
    public void Setup()
    {
        // Load configuration from appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\nural\\RiderProjects\\backend\\Backend\\TodoServices\\appsettings.json",
                optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();

        // Ensure _context is initialized properly
        _context = CreateTestContext();
        if (_context == null)
        {
            throw new Exception("_context is null");
        }

        // Ensure _todoDAO is initialized properly
        _todoDao = new TodoDao(_context);
        if (_todoDao == null)
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
            var dao = new TodoDao(context);
            var user = new User
            {
                Id = Guid.NewGuid(),
            };

            // Convert local time to UTC
            var localNow = DateTime.Now;
            var utcNow = localNow.ToUniversalTime();

            var todo = new Todo
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
        var originalTodo = new Todo
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
        var updatedTodo = new Todo
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
        await _todoDao.UpdateAsync(updatedTodo);

        // Assert
        var retrievedTodo = await _context.Todos.FindAsync(originalTodo.Id);
        Assert.IsNotNull(retrievedTodo);
        Assert.AreEqual(updatedTodo.Name, retrievedTodo.Name);
        Assert.AreEqual(updatedTodo.Description, retrievedTodo.Description); // Compare descriptions
        Assert.AreEqual(updatedTodo.Deadline, retrievedTodo.Deadline);
        Assert.AreEqual(updatedTodo.Status, retrievedTodo.Status);
    }
    
    [Test]
    public async Task GetByIdAsync_ExistingTodoId_ShouldReturnTodo()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Name = "Test ToDo",
            Description = "Test description",
            Deadline = DateTime.UtcNow.AddDays(1),
            Status = Status.Started
        };

        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        // Act
        var retrievedTodo = await _todoDao.GetByIdAsync(todo.Id);

        // Assert
        Assert.IsNotNull(retrievedTodo);
        Assert.AreEqual(todo.Id, retrievedTodo.Id);
        Assert.AreEqual(todo.Name, retrievedTodo.Name);
        Assert.AreEqual(todo.Description, retrievedTodo.Description);
        Assert.AreEqual(todo.UserId, retrievedTodo.UserId);
    }
    
    [Test]
    public async Task DeleteAsync_ExistingTodoId_ShouldDeleteTodo()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Name = "Test ToDo",
            Description = "Test description",
            Deadline = DateTime.UtcNow.AddDays(1),
            Status = Status.Started
        };

        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        // Act
        await _todoDao.DeleteAsync(todo.Id);

        // Assert
        var deletedTodo = await _context.Todos.FindAsync(todo.Id);
        Assert.IsNull(deletedTodo);
    }

    // Unit test for GetAllAsync method
    [Test]
    public async Task GetAllAsync_ShouldReturnAllTodos()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var todo1 = new Todo
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Name = "Test ToDo 1",
            Description = "Test description 1",
            Deadline = DateTime.UtcNow.AddDays(1),
            Status = Status.Started
        };
        var todo2 = new Todo
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Name = "Test ToDo 2",
            Description = "Test description 2",
            Deadline = DateTime.UtcNow.AddDays(2),
            Status = Status.Finished
        };

        await _context.Todos.AddRangeAsync(todo1, todo2);
        await _context.SaveChangesAsync();

        // Act
        var todos = await _todoDao.GetAllAsync();

        // Assert
        Assert.IsNotNull(todos);
        Assert.AreEqual(12, todos.Count());
    }

}