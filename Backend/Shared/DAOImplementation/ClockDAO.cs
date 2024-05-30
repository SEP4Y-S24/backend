using System.Linq.Expressions;
using Shared.IDAO;
using Shared.Context;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shared.DAOImplementation;

public class ClockDAO: IClockDAO
{
    private readonly ClockContext _context;

    public ClockDAO(ClockContext dbContext)
    {
        this._context = dbContext;
    }

    public async Task<Clock> CreateAsync(Clock clock)
    {
        EntityEntry<Clock> added = await _context.Clocks.AddAsync(clock);
        await _context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Clock>> GetAllAsync()
    {
        return await _context.Set<Clock>().ToListAsync();

    }

    public async Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter)
    {
        return await _context.Set<Clock>().Where(filter).ToListAsync();

    }

    public async Task<Clock> UpdateAsync(Clock clockToUpdate)
    {
        Clock? existing =  await _context.Clocks.FirstOrDefaultAsync(post => post.Id == clockToUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Clock with id {clockToUpdate.Id} does not exist!");
        }

        _context.Entry(existing).CurrentValues.SetValues(clockToUpdate);

        _context.Clocks.Update(existing);

        await _context.SaveChangesAsync();

        return clockToUpdate;
    }

    public async Task<Clock?> GetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = await _context.Clocks.FirstOrDefaultAsync(t => t.Id == clockId);
        return existing;
    }
    public async Task<long> GetOffsetByIdAsync(Guid clockId)
    {
        if (clockId.Equals(null))
        {
            throw new ArgumentNullException("Clock's id is null!");
        }
        Clock? existing = await _context.Clocks.FirstOrDefaultAsync(t => t.Id == clockId);
        return existing.TimeOffset;
    }
    public async Task DeleteAsync(Guid id)

    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new ArgumentException("Clock is null!");
        }

        _context.Remove(entity);

        await _context.SaveChangesAsync();

    }
}