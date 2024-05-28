using Models;

namespace Shared.IService;

public interface IMessageService
{
    Task<Message> SendMessage(Message message);
}