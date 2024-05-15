using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserService.Context;
using UserService.IDAO;
using UserService.Model;

namespace EfcDatabase.DAOImplementation;

public class ToDoDAO: IToDoDAO
{
    private readonly UserContext context;
    
    public ToDoDAO(UserContext dbContext)
    {
        context = dbContext;
    }
    
    public async Task<ToDo> CreateAsync(ToDo todo)
    {
        EntityEntry<ToDo> added = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }


    public Task<ToDo?> GetByIdAsync(Guid todoId)
    {
        if (todoId.Equals(null))
        {
            throw new ArgumentNullException("The given Todo ID is null");
        }
        
        ToDo? existing = context.Todos.FirstOrDefault(todo => todo.Id == todoId);
        return Task.FromResult(existing);
    }
    
}