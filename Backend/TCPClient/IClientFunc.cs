namespace TCPClient;

public interface IClientFunc
{
    Task<int> SendMessageAsync(string message,Guid clockId);
    Task<int> SendTMAsync();
    Task<bool> CheckClockIdAsync(Guid clockId);

}