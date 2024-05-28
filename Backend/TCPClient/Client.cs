using System.Net.Sockets;
using System.Text;

namespace TCPClient;

public class Client : IClientFunc
{
    private static NetworkStream _networkStream;
    public Client() 
    {
        StartServer();
    }

    private async void StartServer()
    {
        try
        {
            string serverAddress = "localhost";
            int serverPort = 8080;
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(serverAddress,serverPort);
                using (var networkStream = client.GetStream())
                {
                    Encryption encryption = new Encryption();
                    // Send message to the server
                    var key = $"PK|{encryption.GetPublicKeyString().Length}|{encryption.GetPublicKeyString()}|";
                    encryption.Encrypt(key);
                    
                    networkStream.Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
                    Console.WriteLine(key);
                    var bytesRead = 0;
                    // Wait for the server response
                    byte[] buffer = new byte[1024];
                    while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        //var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // Console.WriteLine($"Received: {receivedData}");
                        var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        if (receivedData == "\r\n")
                        {
                            Console.WriteLine("Exiting...");
                        }
                        else if (receivedData.Length >= 2)
                        {
                            handleServerResponse(receivedData, encryption);
                        }
                        // var response = Encoding.UTF8.GetBytes($"Echo: {receivedText}");
                        // await networkStream.WriteAsync(response, 0, response.Length);
                    }                    
                    string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (logging, rethrowing, etc.)
            throw new InvalidOperationException("An error occurred while communicating with the socket server.", ex);
        }
    }

    public void handleServerResponse(string response, Encryption encryption)
    {
        if (response.Substring(0, 2).ToUpper().Equals("CK"))
        {
            // Clock Key response
            // handle clock key response
           // KeyRequestHandle(response, encryption);
           var publicKey = response.Substring(6, response.Length - 2);
          // encryption.GenerateAesKey(publicKey);
        }
        else
        {
            var cipherText = Encoding.ASCII.GetBytes(response.Substring(16));
            var iv = Encoding.ASCII.GetBytes(response.Substring(0, 16));
            var decryptedText = encryption.Decrypt(cipherText, iv);
            switch (decryptedText.Substring(0, 2).ToUpper())
            {
                case "TM":
                    // Time request
                    //   TimeRequestHandle(stream, encryption);
                    break;
                case "MS":
                    // Message response
                    // handle message response
                    // MessageResponseHandle(decryptedText, stream, encryption);
                    break;
                case "AU":
                    // Authentication response
                    // handle authentication response
                    //     AuthenticationRequestHandle(decryptedText, stream, encryption);
                    break;
                default:
                    // Unknown command
                    throw new InvalidOperationException("Unknown command received.");
            }
        }
    }

    public async Task<int> SendMessageAsync(string message,Guid clockId)
    {
        Encryption encryption = new Encryption();
        // Send message to the server
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