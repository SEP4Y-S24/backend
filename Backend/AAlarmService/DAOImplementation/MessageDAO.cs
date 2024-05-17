using AAlarmServices.IDAO;
using AlarmServices.Context;
using AlarmServices.Model;
using Microsoft.EntityFrameworkCore;

namespace AAlarmServices.DAOImplementation;

public class MessageDAO: IMessageDAO
{
    private readonly AlarmContext _alarmContext;

    public MessageDAO(AlarmContext dbContext)
    {
        this._alarmContext = dbContext;
    }

    public async Task<Message> CreateAsync(Message message)
    {
        try
        {
            User? user = _alarmContext.Set<User>().FirstOrDefault(c => c.Id.Equals(message.SenderId));
            if (user is null)
            {
                throw new ArgumentNullException("Sender is missing from message!");
            }
            message.Sender = user;
            User? reciever = _alarmContext.Set<User>().FirstOrDefault(c => c.Id.Equals(message.ReceiverId));
            if (reciever is null)
            {
                throw new ArgumentNullException("Reciever user is missing from message!");
            }
            message.Reciever = reciever;
            Clock? clock = _alarmContext.Set<Clock>().Include(c=>c.Owner).FirstOrDefault(c => c.Id == message.ClockId);
            if (clock is null)
            {
                throw new ArgumentNullException("Recieving clock is missing from message");
            }
            message.Clock = clock;
            await _alarmContext.Messages.AddAsync(message);
            await _alarmContext.SaveChangesAsync();
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
        return await _alarmContext.Set<Message>().ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetAllSentMessagesByUserIdAsync(Guid userId)
    {
        User? user = _alarmContext.Set<User>().FirstOrDefault(user => user.Id.Equals(userId));
        if (user==null)
        {
            throw new ArgumentNullException("The user Id is null.");
        }

        IEnumerable<Message> sentMessages = await _alarmContext.Set<Message>()
            .Where(m => m.SenderId == userId).ToListAsync();
        return sentMessages;
    }

    public async Task<IEnumerable<Message>> GetAllReceivedMessagesByUserIdAsync(Guid userId)
    {
        User? user = _alarmContext.Set<User>().FirstOrDefault(user => user.Id.Equals(userId));
        if (user==null)
        {
            throw new ArgumentNullException("The user Id is null.");
        }

        IEnumerable<Message> sentMessages = await _alarmContext.Set<Message>()
            .Where(m => m.ReceiverId == userId).ToListAsync();
        return sentMessages;
    }

    public async Task UpdateAsync(Message message)
    {
        Message dbEntity = await GetByIdAsync(message.Id);

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
        
        _alarmContext.Update(dbEntity);

        await _alarmContext.SaveChangesAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid messageId)
    {
        return await _alarmContext.Set<Message>().Include(m=>m.Reciever).Include(m=>m.Sender).SingleAsync(e => e.Id == messageId);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}