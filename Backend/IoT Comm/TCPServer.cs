using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoT_Comm;

public class TCPServer
{
    private TcpListener _tcpListener;

    public TCPServer()
    {
        StartServer();
    }

    private void StartServer()
    {
        var port = 13000;
        var hostAddress = IPAddress.Parse("127.0.0.1");
        
        _tcpListener = new TcpListener(hostAddress, port);
        _tcpListener.Start();

        byte[] buffer = new byte[256];
        string receivedData;
        
        using TcpClient client = _tcpListener.AcceptTcpClient();
        
        var stream = client.GetStream();

        int readTotal;
        
        while ((readTotal = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            receivedData = Encoding.UTF8.GetString(buffer, 0, readTotal);
            if (receivedData.Length > 2)
            {
                IdentifyCommand(receivedData);
                var response= Encoding.UTF8.GetBytes(receivedData);
                stream.Write(response, 0, receivedData.Length);
            }
        }
    }
    
    private void IdentifyCommand(string receivedData)
    {
        switch (receivedData.Substring(0,2))
        {
            case "TM":
                // Time request
                // handle time request
                Console.WriteLine("Time request received.");
                break;
            case "MS":
                // Message response
                // handle message response
                Console.WriteLine("Message response received.");
                break;
            default:
                // Unknown command
                throw new InvalidOperationException("Unknown command received.");
        }
    }
}