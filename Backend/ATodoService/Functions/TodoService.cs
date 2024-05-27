using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.dtos;
using Shared.DTOs;

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
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos/users/{userId}")] HttpRequest req, Guid userId)
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
                    Name = todo.Name,  // Mapping the existing properties
                    Description = todo.Description,
                    Deadline = todo.Deadline,
                    Status = todo.Status,
                    UserId = todo.UserId,
                    
                };
                foreach (var tag in todo.Tags)
                {
                    TagDto tagDto = new TagDto()
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        UserId = tag.UserId
                    };
                    todoDto.Tags.tags.Add(tagDto);
                }
                todosDto.Todos.Add(todoDto);
            }
            return new OkObjectResult(todosDto);
        }


        [Function("CreateTodo")]
        public async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todo")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TodoDto todo = JsonConvert.DeserializeObject<TodoDto>(requestBody);
            var todoService = ServiceFactory.GetTodoService();
            var todoDao = ServiceFactory.GetTodoDAO();

            Todo todoToCreate = new Todo
            {
                Id = Guid.NewGuid(),
                Name = todo.Name,
                Description = todo.Description,
                Deadline = todo.Deadline,
                Status = todo.Status,
                UserId = todo.UserId
            };
            Todo created = await todoService.CreateAsync(todoToCreate);
            TodoDto todoDto = new TodoDto
            {
                Name = created.Name,
                Description = created.Description,
                Deadline = created.Deadline,
                Status = created.Status,
                UserId = created.UserId
            };
            return new OkObjectResult(todoDto);
        }

        [Function("GetTodo")]
        public async Task<IActionResult> GetTodo(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var todoService = ServiceFactory.GetTodoService();
            Todo todo = await todoService.GetByIdAsync(id);

            TodoDto todoDto = new TodoDto
            {
                Name = todo.Name,
                Description = todo.Description,
                Deadline = todo.Deadline,
                Status = todo.Status,
                UserId = todo.UserId
            };
            todoDto.Tags = new Tags();
            foreach (var tag in todo.Tags)
            {
                TagDto tagDto = new TagDto()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    UserId = tag.UserId
                };
                todoDto.Tags.tags.Add(tagDto);
            }
            return new OkObjectResult(todoDto);
        }
        
        [Function("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "todo/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TodoDto todoDto = JsonConvert.DeserializeObject<TodoDto>(requestBody);

            var todoService = ServiceFactory.GetTodoService();
            var existingTodo = await todoService.GetByIdAsync(id);
            if (existingTodo == null)
            {
                return new NotFoundResult();
            }

            existingTodo.Name = todoDto.Name;
            existingTodo.Description = todoDto.Description;
            existingTodo.Deadline = todoDto.Deadline;
            existingTodo.Status = todoDto.Status;
            existingTodo.UserId = todoDto.UserId;

            await todoService.UpdateAsync(existingTodo);

            return new OkObjectResult(todoDto);
        }
        
        [Function("DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todo/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var todoService = ServiceFactory.GetTodoService();
            await todoService.DeleteAsync(id);
            return new OkResult();
        }

        [Function("UpdateTodoStatus")]
        public async Task<IActionResult> UpdateTodoStatus(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "todo/tag/{id}/status")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            StatusUpdateDto statusUpdateDto = JsonConvert.DeserializeObject<StatusUpdateDto>(requestBody);

            var todoService = ServiceFactory.GetTodoService();
            await todoService.UpdateStatusByIdAsync(id, statusUpdateDto.Status);
            return new OkResult();
        }

        [Function("AddTagToTask")]
        public async Task<IActionResult> AddTagToTask([HttpTrigger(AuthorizationLevel.Function, "patch", 
            Route = "todo/{todoId}/tag")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            Tags tags = JsonConvert.DeserializeObject<Tags>(requestBody);
            var tagService = ServiceFactory.GetTagService();

     
            await tagService.addTaskToTagAsync(tags.tags, id);
            
            return new OkResult();
        }
    }
}
