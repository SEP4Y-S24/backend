using Application.DAO;
using EfcDatabase.Context;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Services.Services;

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

    public Task UpdateAsync(Clock clockToUpdate)
    {
        Clock? existing = context.Clocks.FirstOrDefault(post => post.Id == clockToUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Clock with id {clockToUpdate.Id} does not exist!");
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

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}