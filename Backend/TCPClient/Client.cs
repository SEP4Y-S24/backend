using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace TCPClient;

public class Client : IClientFunc
{
    private static NetworkStream _networkStream;
    private static Encryption encryption;
    public Client() 
    {
        encryption = new Encryption();
        StartServer();
    }

    public async void StartServer()
    {
        try
        {
            string serverAddress = "4.184.192.42";
            int serverPort = 13000;
            var client = new TcpClient(); 
            await client.ConnectAsync(serverAddress,serverPort);
            _networkStream = client.GetStream();
        }
        catch (Exception ex)
        {
            // Handle exceptions (logging, rethrowing, etc.)
            throw new InvalidOperationException("An error occurred while communicating with the socket server.", ex);
        }
    }
    

    public async Task<int> SendTMAsync()
    {
        while (_networkStream == null)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("Sending TM...");
        // Send message to the server
        var key = $"TM|";
        var cipherText =encryption.Encrypt(key);
        Console.WriteLine(key);
        Console.WriteLine(cipherText.Length);
         _networkStream.Write(cipherText, 0, cipherText.Length);
        Console.WriteLine(key);
        var bytesRead = 0;
        // Wait for the server response
        byte[] buffer = new byte[1024];
        bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
        var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        if (receivedData == "\r\n")
        {
            Console.WriteLine("Exiting...");
        }
        else if (receivedData.Length >= 2)
        {
            if (receivedData.Substring(0, 2).ToUpper().Equals("OK")) return 200;
            else return 400;
        }

        return 400;
    }
    public async Task<bool> CheckClockIdAsync(Guid clockId)
    {
        while (_networkStream == null)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("CheckClockIdAsync...");
        // Send message to the server
        var key = $"IR|{clockId}|";
        Console.WriteLine(key);
//        Console.WriteLine(cipherText.Length);
        _networkStream.Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
        Console.WriteLine(key);
        var bytesRead = 0;
        // Wait for the server response
        byte[] buffer = new byte[1024];
        bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
        var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        if (receivedData == "\r\n")
        {
            Console.WriteLine("Exiting...");
        }
        else if (receivedData.Length >= 2)
        {
            if (receivedData.Split('|')[1].Equals("1")) return true;
            else return false;
        }

        return false;
    }

    public async Task<int> SendMessageAsync(string message,Guid clockId)
    {
        // Send message to the server
        while (_networkStream == null)
        {
            Thread.Sleep(1000);
        }

        var key = $"MS|{message.Length+17}|{clockId.ToString()}|{message}|";
        _networkStream.Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
        Console.WriteLine(key);
        var bytesRead = 0;
        // Wait for the server response
        byte[] buffer = new byte[1024];
        bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
        var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        if (receivedData == "\r\n")
        {
            Console.WriteLine("Exiting...");
        }
        else if (receivedData.Length >= 2)
        {
            if (receivedData.Substring(0, 2).ToUpper().Equals("OK")) return 200;
            else return 400;
        }

        return 400;
    }
}