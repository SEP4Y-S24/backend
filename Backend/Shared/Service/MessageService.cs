using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class MessageService : IMessageService
{
    private readonly IMessageDao _messageDao;
    //private readonly IOTInterface interface;
    public MessageService(IMessageDao messageDao)
    {
        this._messageDao = messageDao;
    }
    public async Task<Message> SendMessage(Message message)
    {
        try
        {
            Message m = await _messageDao.CreateAsync(message);
            /*
             * Send the message to IOTComm
             */
            return m;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}