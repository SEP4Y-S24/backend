using Microsoft.AspNetCore.Mvc;
using TodoServices.dtos;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateAsync(CreateTodo todo)
    {
        try
        {
            Todo todoToCreate = new Todo()
            {
                Id = Guid.NewGuid(),
                Name = todo.Name,
                UserId = todo.UserId,
                Status = todo.Status,
                Deadline = todo.Deadline,
                Description = todo.Description
            };
            Todo created = await _todoService.CreateAsync(todoToCreate);
            TodoDto todoDto = new TodoDto()
            {
                Id = created.Id,
                Name = created.Name,
                UserId = created.UserId,
                Status = created.Status,
                Deadline = created.Deadline,
                Description = created.Description
            };
            return Created($"/todos/{created.Id}", todoDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetByIdAsync(Guid id)
    {
        try
        {
            Todo? todo = await _todoService.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            TodoDto todoDto = new TodoDto()
            {
                Id = todo.Id,
                Name = todo.Name,
                UserId = todo.UserId,
                Status = todo.Status,
                Deadline = todo.Deadline,
                Description = todo.Description
            };
            return Ok(todoDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("/tags")]
    public async Task<ActionResult<Todo>> GetAllByTagsAsync(Tags tags)
    {
        try
        {
            List<Tag> tagsList = new List<Tag>();
            foreach (var t in tags.tags)
            {
                Tag tag = new Tag()
                {
                    name = t.Name
                };
                tagsList.Add(tag);
            }
            IEnumerable<Todo>? todo = await _todoService.FilterByTags(tagsList);
            if (todo == null)
            {
                return NotFound();
            }
            TodosDto todosDto = new TodosDto();
            foreach (var t in todo)
            {
                TodoDto todoDto = new TodoDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    UserId = t.UserId,
                    Status = t.Status,
                    Deadline = t.Deadline,
                    Description = t.Description
                };
                todosDto.Todos.Add(todoDto);
            }
            return Ok(todosDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpPut]
    public async Task<ActionResult<Todo>> UpdateAsync(TodoDto todo)
    {
        try
        {
            Todo todoToUpdate = new Todo()
            {
                Id = Guid.NewGuid(),
                Name = todo.Name,
                UserId = todo.UserId,
                Status = todo.Status,
                Deadline = todo.Deadline,
                Description = todo.Description
            };
            if (todo == null)
            {
                return NotFound();
            }

            await _todoService.UpdateAsync(todoToUpdate);
            TodoDto todoDto = new TodoDto()
            {
                Id = todo.Id,
                Name = todo.Name,
                UserId = todo.UserId,
                Status = todo.Status,
                Deadline = todo.Deadline,
                Description = todo.Description
            };
            return Ok(todoDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateStatusByIdAsync(Guid todoId, Status status)
    {
        try
        {
            if (todoId == null)
            {
                return NotFound();
            }

            await _todoService.UpdateStatusByIdAsync(todoId, status);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(Guid todoId)
    {
        try
        {
            await _todoService.DeleteAsync(todoId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}