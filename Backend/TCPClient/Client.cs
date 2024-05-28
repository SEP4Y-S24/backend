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
    }

    public async void StartServer()
    {
        try
        {
            string serverAddress = "10.154.208.67";
            int serverPort = 13000;
            var client = new TcpClient();
            
                 await client.ConnectAsync(serverAddress,serverPort);
                _networkStream = client.GetStream();
                
                    // Send message to the server
                    var key = $"CK|{encryption.GetPublicKeyString().Length}|{encryption.GetPublicKeyString()}|";
                    
                    await _networkStream.WriteAsync(Encoding.ASCII.GetBytes(key), 0, key.Length);
                    Console.WriteLine(key);
                    var bytesRead = 0;
                    // Wait for the server response
                    byte[] buffer = new byte[1024];
                    bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
                    Console.WriteLine("Bytes read");
                        //var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // Console.WriteLine($"Received: {receivedData}");
                        var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine(receivedData);
                        if (receivedData == "\r\n")
                        {
                            Console.WriteLine("Exiting...");
                        }
                        else if (receivedData.Length >= 2)
                        {
                            handleServerResponse(receivedData, encryption);
                            Console.WriteLine("receivedData");

                        }
                        // var response = Encoding.UTF8.GetBytes($"Echo: {receivedText}");
                        // await networkStream.WriteAsync(response, 0, response.Length);
                                       
                    string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        catch (Exception ex)
        {
            // Handle exceptions (logging, rethrowing, etc.)
            throw new InvalidOperationException("An error occurred while communicating with the socket server.", ex);
        }
    }

    public void handleServerResponse(string response, Encryption encryption)
    {
        if (response.Substring(0, 2).ToUpper().Equals("SK"))
        {
            // Clock Key response
            // handle clock key response
           // KeyRequestHandle(response, encryption);
           var publicKey = response.Split("|")[2];
           byte[] key = Convert.FromBase64String(publicKey);
           encryption.GenerateAesKey(key);
        }
    }

    public async Task<int> SendTMAsync()
    {
        while (encryption.IsAesKeyNull())
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

    public async Task<int> SendMessageAsync(string message,Guid clockId)
    {
        // Send message to the server
        while (_networkStream == null)
        {
            Thread.Sleep(1000);
        }

        var key = $"SM|{message.Length+17}|{clockId.ToString()}|{message}|";
        encryption.Encrypt(key);

        //      networkStream.Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
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