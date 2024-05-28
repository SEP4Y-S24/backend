using Microsoft.EntityFrameworkCore;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

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
            User? user = await _context.Set<User>().FirstOrDefaultAsync(c => c.Id.Equals(message.SenderId));
            if (user is null)
            {
                throw new ArgumentNullException("Sender is missing from message!");
            }
            message.Sender = user;
            User? reciever = await _context.Set<User>().FirstOrDefaultAsync(c => c.Id.Equals(message.ReceiverId));
            if (reciever is null)
            {
                throw new ArgumentNullException("Reciever user is missing from message!");
            }
            message.Reciever = reciever;
            Clock? clock = await _context.Set<Clock>().Include(c=>c.Owner).FirstOrDefaultAsync(c => c.Id == message.ClockId);
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

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Set<Message>().ToListAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        if (message==null || message.Id.Equals(null))
        {
            throw new ArgumentException();
        }
        Message? dbEntity = await GetByIdAsync(message.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.ReceiverId = message.ReceiverId;
        dbEntity.Reciever = message.Reciever;
        dbEntity.SenderId = message.SenderId;
        dbEntity.ClockId = message.ClockId;
        dbEntity.Clock = message.Clock;
        dbEntity.Sender = message.Sender;
        
        _context.Update(dbEntity);

        await _context.SaveChangesAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid messageId)
    {
        return await _context.Set<Message>().Include(m=>m.Reciever).Include(m=>m.Sender).SingleAsync(e => e.Id == messageId);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}