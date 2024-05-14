using Microsoft.AspNetCore.Mvc;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController: ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateAsync(Todo todo)
    {
        try
        {
            Todo todoToCreate = new Todo()
            {
                Id = Guid.NewGuid(),
                Name = todo.Name,
                User = todo.User,
                UserId = todo.UserId,
                Status = todo.Status,
                Deadline = todo.Deadline,
                Description = todo.Description
            };
            Todo created = await _todoService.CreateAsync(todo);
            return Created($"/todos/{created.Id}", created);
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
        
                return Ok(todo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<Todo>> UpdateAsync(Todo todo)
        {
            try
            {
                if (todo == null)
                {
                    return NotFound();
                }
                
                await _todoService.UpdateAsync(todo);
                return Ok(todo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
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