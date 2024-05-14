using System.Linq.Expressions;
using ClockServices.Context;
using ClockServices.IDAO;
using ClockServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClockServices.DAOImplementation;

public class ClockDAO : IClockDAO
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
        return await context.Set<Clock>().Include(e => e.Messages).ToListAsync();

    }

    public async Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter)
    {
        return await context.Set<Clock>().Include(e => e.Messages).Where(filter).ToListAsync();

    }

    public async Task<Clock> UpdateAsync(Clock clockToUpdate)
    {
        Clock? existing =  context.Clocks.FirstOrDefault(post => post.Id == clockToUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Clock with id {clockToUpdate.Id} does not exist!");
        }

        context.Entry(existing).CurrentValues.SetValues(clockToUpdate);
        if (clockToUpdate.Messages != null)
        {
            existing.Messages = new List<Message>();
            foreach (var message in clockToUpdate.Messages)
            {
                clockToUpdate.Messages.Add(message);
            }
        }

        context.Clocks.Update(existing);

        context.SaveChanges();

        return clockToUpdate;
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
    public Task<long> GetOffsetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = context.Clocks.FirstOrDefault(t => t.Id == clockId);
        return Task.FromResult(existing.TimeOffset); ;
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