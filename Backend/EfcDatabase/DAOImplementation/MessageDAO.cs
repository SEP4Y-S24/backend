using EfcDatabase.IDAO;
using EfcDatabase.Context;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace EfcDatabase.DAOImplementation;

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
            User? user = _context.Set<User>().FirstOrDefault(c => c.Id.Equals(message.SenderId));
            if (user is null)
            {
                throw new ArgumentNullException("Sender is missing from message!");
            }
            message.Sender = user;
            Clock? clock = _context.Set<Clock>().Include(c=>c.Owner).FirstOrDefault(c => c.Id == message.ReceiverId);
            if (clock is null)
            {
                throw new ArgumentNullException("Reciever is missing from message");
            }
            message.Reciever = clock;
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