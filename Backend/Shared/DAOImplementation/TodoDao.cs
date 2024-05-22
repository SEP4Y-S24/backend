using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

public class TodoDao: ITodoDAO
{
    private readonly ClockContext _context;
    
    public TodoDao(ClockContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Todo> CreateAsync(Todo todo)
    {
        EntityEntry<Todo> added = await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(Todo todo)
    {
        Todo? dbEntity = await GetByIdAsync(todo.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        _context.Entry(dbEntity).CurrentValues.SetValues(todo);

        _context.Todos.Update(dbEntity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusByIdAsync(Guid todoId, Status status)
    {
        Todo? dbEntity = await GetByIdAsync(todoId);
        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.Status = status;
        _context.Todos.Update(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Todo?> GetByIdAsync(Guid todoId)
    {
        if (todoId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Todo ID is null");
        }
        
        Todo? existing = await _context.Todos.Include(t=>t.Tags).FirstOrDefaultAsync(todo => todo.Id == todoId);
        return existing;
    }

    public async Task DeleteAsync(Guid todoId)
    {
        if (todoId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        Todo? toDelete = await GetByIdAsync(todoId);
        if (toDelete == null)
        {
            throw new ArgumentNullException("Todo object is not found in the database");
        }
        _context.Todos.Remove(toDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _context.Set<Todo>().Include(t=>t.Tags).ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllByAsync(Expression<Func<Todo, bool>> filter)
    {
        return await _context.Set<Todo>().Include(t=>t.Tags).Where(filter).ToListAsync();
    }
    
}