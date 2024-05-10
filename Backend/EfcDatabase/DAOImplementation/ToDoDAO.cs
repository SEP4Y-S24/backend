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

    public async Task UpdateAsync(ToDo todo)
    {
        ToDo dbEntity = await GetByIdAsync(todo.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.User = todo.User;
        dbEntity.UserId = todo.UserId;
        dbEntity.Name = todo.Name;
        dbEntity.Deadline = todo.Deadline;
        dbEntity.Status = todo.Status;
        
        context.Update(dbEntity);

        await context.SaveChangesAsync();
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

    public async Task DeleteAsync(Guid todoId)
    {
        if (todoId==null)
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        ToDo? toDelete = await GetByIdAsync(todoId);
        context.Todos.Remove(toDelete);
        await context.SaveChangesAsync();
    }
}