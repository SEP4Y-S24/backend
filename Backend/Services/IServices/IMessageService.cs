using EfcDatabase.Model;

namespace Services.IServices;

public interface IMessageService
{
    Task<Message> SendMessage(Message message);
}