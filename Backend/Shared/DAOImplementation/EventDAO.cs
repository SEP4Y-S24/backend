using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

public class EventDAO : IEventDAO
{
    private readonly ClockContext _context;
    
    public EventDAO(ClockContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<Event> CreateAsync(Event entity)
    {
        if(entity.StartDate>entity.EndDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date");
        }
        EntityEntry<Event> added = await _context.Events.AddAsync(entity);
        await _context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(Event entity)
    {
        if(entity.StartDate>entity.EndDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date");
        }

        Event? dbEntity = await GetByIdAsync(entity.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        _context.Entry(dbEntity).CurrentValues.SetValues(entity);

        _context.Events.Update(dbEntity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusByIdAsync(Guid eventId, Status status)
    {
        Event? dbEntity = await GetByIdAsync(eventId);
        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.Status = status;
        _context.Events.Update(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Event?> GetByIdAsync(Guid eventId)
    {
        if (eventId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Todo ID is null");
        }
        
        Event? existing = await _context.Events.Include(t=>t.Categories).FirstOrDefaultAsync(ev => ev.Id == eventId);
        return existing;
    }

    public async Task DeleteAsync(Guid eventId)
    {
        if (eventId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        Event? entity = await GetByIdAsync(eventId);
        if (entity == null)
        {
            throw new ArgumentNullException("Todo object is not found in the database");
        }
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Set<Event>().Include(t=>t.Categories).ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Set<Event>()
            .Where(t => t.UserId == userId)
            .Include(t => t.Categories)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetAllByAsync(Expression<Func<Event, bool>> filter)
    {
        return await _context.Set<Event>().Include(t=>t.Categories).Where(filter).ToListAsync();
    }
}