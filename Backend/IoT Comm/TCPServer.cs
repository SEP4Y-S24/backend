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
            if (receivedData == "\r\n")
            {
                Console.WriteLine("Exiting...");
            }
            else if (receivedData.Length >= 2)
            {
                IdentifyCommand(receivedData, stream, buffer);
                
            }
        }
    }
    
    private void IdentifyCommand(string receivedData, NetworkStream stream, byte[] buffer)
    {
        switch (receivedData.Substring(0,2).ToUpper())
        {
            case "TM":
                // Time request
                // handle time request
                TimeRequestHandle(receivedData, stream, buffer);
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
    
    private void TimeRequestHandle(string receivedData, NetworkStream stream, byte[] buffer)
    {
        var time = DateTime.Now.ToString("HH:mm:ss");
        var response= "TM\r\n1\r\n8\r\n" + time + "\r\n";
        byte[] timeBytes = Encoding.UTF8.GetBytes(response);
        stream.Write(timeBytes, 0, timeBytes.Length);
        Console.WriteLine("Time request received.");
    }
}