using EfcDatabase.Model;
using Microsoft.AspNetCore.Mvc;
using Services.IServices;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController: ControllerBase
{
    private readonly IToDoService _toDoService;

    public TodoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpPost]
    public async Task<ActionResult<ToDo>> CreateAsync([FromBody] ToDo todo)
    {
        try
        {
            ToDo created = await _toDoService.CreateAsync(todo);
            return Created($"/todos/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}