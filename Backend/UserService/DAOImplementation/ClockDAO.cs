using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserService.Context;
using UserService.IDAO;
using UserService.Model;

namespace UserService.DAOImplementation;

public class ClockDAO : IClockDAO
{
    private readonly UserContext context;

    public ClockDAO(UserContext dbContext)
    {
        this.context = dbContext;
    }

    public async Task<Clock> CreateAsync(Clock clock)
    {
        EntityEntry<Clock> added = await context.Clocks.AddAsync(clock);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter)
    {
        return await context.Set<Clock>().Where(filter).ToListAsync();
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
    
}