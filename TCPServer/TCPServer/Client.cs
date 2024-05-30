using System.Net.Sockets;

namespace TCPServer;

public class Client
{
    public Guid Id { get; set; }
    public TcpClient TcpClient { get; set; }
    public Encryption Encryption { get; set; }
    
    public Client(TcpClient tcpClient)
    {
        Id=Guid.Empty;
        TcpClient = tcpClient;
        Encryption = new Encryption();
    }
    
}