using ClockServices.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoServices.IDAO;
using TodoServices.Model;

namespace TodoServices.DAOImplementation;

public class TodoDAO: ITodoDao
{
    private readonly ClockContext context;
    
    public TodoDAO(ClockContext dbContext)
    {
        context = dbContext;
    }
    
    public async Task<Todo> CreateAsync(Todo todo)
    {
        EntityEntry<Todo> added = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(Todo todo)
    {
        Todo? dbEntity = await GetByIdAsync(todo.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        context.Entry(dbEntity).CurrentValues.SetValues(todo);

        context.Todos.Update(dbEntity);

        await context.SaveChangesAsync();
    }

    public Task<Todo?> GetByIdAsync(Guid todoId)
    {
        if (todoId.Equals(null))
        {
            throw new ArgumentNullException("The given Todo ID is null");
        }
        
        Todo? existing = context.Todos.FirstOrDefault(todo => todo.Id == todoId);
        return Task.FromResult(existing);
    }

    public async Task DeleteAsync(Guid todoId)
    {
        if (todoId==null)
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        Todo? toDelete = await GetByIdAsync(todoId);
        context.Todos.Remove(toDelete);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await context.Set<Todo>().ToListAsync();
    }
}