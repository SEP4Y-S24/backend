using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Shared;
using Shared.dtos;

namespace ATodoService.Functions
{
    public class TodoService
    {
        private readonly ILogger<TodoService> _logger;

        public TodoService(ILogger<TodoService> logger)
        {
            _logger = logger;
        }

        [Function("TodoService")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("GetAllTodos")]
        public async Task<IActionResult> GetAllTodos(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos/users/{userId}")] HttpRequest req,
            Guid userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var todoService = ServiceFactory.GetTodoService();
            IEnumerable<Todo> todos = await todoService.GetAllByUserIdAsync(userId);
            TodosDto todosDto = new TodosDto();
            todosDto.Todos = new List<TodoDto>();

            foreach (var todo in todos)
            {
                TodoDto todoDto = new TodoDto()
                {
                    Name = "Hello azure!",
                    Description = "asdas",
                    Deadline = DateTime.UtcNow.AddDays(7),
                    Status = Status.Started,
                    UserId = userId
                };
                todosDto.Todos.Add(todoDto);
            }

            return new OkObjectResult(todosDto);
        }
    }
}
