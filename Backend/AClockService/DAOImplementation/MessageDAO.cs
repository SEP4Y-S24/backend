using AClockService.Context;
using AClockService.IDAO;
using AClockService.Model;
using Microsoft.EntityFrameworkCore;

namespace AClockService.DAOImplementation;

public class MessageDAO : IMessageDao
{
    private readonly ClockContext _context;

    public MessageDAO(ClockContext dbContext)
    {
        this._context = dbContext;
    }

    public async Task<Message> CreateAsync(Message message)
    {
        try
        {
            Clock? clock = _context.Set<Clock>().FirstOrDefault(c => c.Id == message.ClockId);
            if (clock is null)
            {
                throw new ArgumentNullException("Recieving clock is missing from message");
            }
            message.Clock = clock;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
        catch (ArgumentNullException e)
        {
            throw new ArgumentNullException(e.Message);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Message>> GetAll()
    {
        return await _context.Set<Message>().ToListAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        Message dbEntity = await GetByIdAsync(message.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.ClockId = message.ClockId;
        dbEntity.Clock = message.Clock;
        
        _context.Update(dbEntity);

        await _context.SaveChangesAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid messageId)
    {
        return await _context.Set<Message>().SingleAsync(e => e.Id == messageId);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}