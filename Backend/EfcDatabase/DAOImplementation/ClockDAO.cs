using System.Linq.Expressions;
using EfcDatabase.IDAO;
using EfcDatabase.Context;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDatabase.DAOImplementation;

public class ClockDAO: IClockDAO
{
    private readonly ClockContext context;

    public ClockDAO(ClockContext dbContext)
    {
        this.context = dbContext;
    }

    public async Task<Clock> CreateAsync(Clock clock)
    {
        EntityEntry<Clock> added = await context.Clocks.AddAsync(clock);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Clock>> GetAll()
    {
        return await context.Set<Clock>().Include(e=> e.Messages).ToListAsync();

    }

    public async Task<IEnumerable<Clock>> GetAllBy(Expression<Func<Clock, bool>> filter)
    {
        return await context.Set<Clock>().Include(e => e.Messages).Where(filter).ToListAsync();

    }

    public Task UpdateAsync(Clock clockToUpdate)
    {
        Clock? existing = context.Clocks.FirstOrDefault(post => post.Id == clockToUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Clock with id {clockToUpdate.Id} does not exist!");
        }

        context.Entry(existing).CurrentValues.SetValues(clockToUpdate);
        existing.Messages = new List<Message>();
        foreach (var message in clockToUpdate.Messages)
        {
            clockToUpdate.Messages.Add(message);
        }

        context.Clocks.Update(clockToUpdate);
    
        context.SaveChanges();
    
        return Task.CompletedTask;
    }

    public Task<Clock?> GetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = context.Clocks.FirstOrDefault(t => t.Id == clockId);
        return Task.FromResult(existing);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new ArgumentException("Clock is null!");
        }

        context.Remove(entity);

        await context.SaveChangesAsync();

    }
}