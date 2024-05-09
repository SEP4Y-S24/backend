using EfcDatabase.Context;
using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDatabase.DAOImplementation;

public class ToDoDAO: IToDoDAO
{
    private readonly ClockContext context;
    
    public ToDoDAO(ClockContext dbContext)
    {
        context = dbContext;
    }
    
    public async Task<ToDo> CreateAsync(ToDo todo)
    {
        EntityEntry<ToDo> added = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public Task UpdateAsync(Guid todoId)
    {
        Task<ToDo?> toUpdate = GetByIdAsync(todoId);
        
        ToDo? existing = context.Todos.FirstOrDefault(todo => todo.Id == todoId);
        if (existing==null)
        {
            throw new Exception($"Todo with id {todoId} does not exist.");
        }

        context.Todos.Update(toUpdate.Result);
        context.SaveChanges();
        return Task.CompletedTask;
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

    public Task DeleteAsync(Guid todoId)
    {
        if (todoId==null)
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        Task<ToDo?> toDelete = GetByIdAsync(todoId);
        context.Todos.Remove(toDelete.Result);
        return Task.CompletedTask;
    }
}