using Models;

namespace Shared.IServices;

public interface IMessageService
{
    Task<Message> SendMessage(Message message);
}