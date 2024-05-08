using Application.DAO;
using EfcDatabase.Context;
using EfcDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace Services.Services;

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

    public Task<IEnumerable<Message>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Message clock)
    {
        throw new NotImplementedException();
    }

    public Task<Message?> GetByIdAsync(Guid messageId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}