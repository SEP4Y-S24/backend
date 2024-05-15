using UserService.Model;

namespace UserService.IServices;

public interface IMessageService
{
    Task<Message> SendMessage(Message message);
}