using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TCPServer;

class Program
{
    
    static async Task Main(string[] args)
    {
        int port = 13000;
        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Server listening on port {port}");
        

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        Console.WriteLine("Client connected.");
        using var networkStream = client.GetStream();
        Encryption encryption = new Encryption();
        var buffer = new byte[1024];
        int bytesRead;
        
        while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        {
            var receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            // Console.WriteLine($"Received: {receivedData}");
            receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            if (receivedData == "\r\n")
            {
                Console.WriteLine("Exiting...");
            }
            else if (receivedData.Length >= 2)
            {
                IdentifyCommand(receivedData, networkStream, encryption);
            }
            // var response = Encoding.UTF8.GetBytes($"Echo: {receivedText}");
            // await networkStream.WriteAsync(response, 0, response.Length);
        }
        
        Console.WriteLine("Client disconnected.");
        client.Close();
    }
    
    private static void IdentifyCommand(string receivedData, NetworkStream stream, Encryption encryption)
    {
        if (receivedData.Substring(0, 2).ToUpper().Equals("CK"))
        {
            // Clock Key response
            // handle clock key response
            KeyRequestHandle(receivedData, stream, encryption);
        }
        else
        {
            var cipherText=Encoding.ASCII.GetBytes(receivedData.Substring(16));
            var iv=Encoding.ASCII.GetBytes(receivedData.Substring(0,16));
            var decryptedText = encryption.Decrypt(cipherText, iv);
            switch (decryptedText.Substring(0,2).ToUpper())
            {
                case "TM":
                    // Time request
                    TimeRequestHandle(stream, encryption);
                    break;
                case "MS":
                    // Message response
                    // handle message response
                    MessageResponseHandle(decryptedText, stream, encryption);
                    break;
                case "AU":
                    // Authentication response
                    // handle authentication response
                    AuthenticationRequestHandle(decryptedText, stream, encryption);
                    break;
                default:
                    // Unknown command
                    throw new InvalidOperationException("Unknown command received.");
            }   
        }
    }
    
    private static async void TimeRequestHandle(NetworkStream stream, Encryption encryption)
    {
        try
        {
            var time = DateTime.Now.ToString("HH:mm");
            time= "TM|1|4|" + time.Replace(":","") + "|";
            byte[] timeBytes = Encoding.ASCII.GetBytes(time);
            stream.Write(timeBytes, 0, timeBytes.Length);
            Console.WriteLine("Time request received.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    private static void MessageResponseHandle(string decryptedText, NetworkStream stream, Encryption encryption)
    {
        try
        {
            var message = decryptedText.Split("|");
            var messageToSend = message[2];
            Console.WriteLine($"Message {messageToSend} request received.");
            // stream.Write();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
   private static void KeyRequestHandle(string decryptedText, NetworkStream stream, Encryption encryption)
    {
        try
        {
            var message = decryptedText.Split("|");
            var clockPublicKey = Encryption.PublicKeyFromString(message[2]);
            encryption.GenerateAesKey(clockPublicKey);
            Console.WriteLine($"Clock Key request received.");
            
            
            //Send SK response
            var key = $"SK|{encryption.GetPublicKeyString().Length}|{encryption.GetPublicKeyString()}|";
            stream.Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
            Console.WriteLine(key);
            // byte[] bytes= Encoding.ASCII.GetBytes();
            // stream.Write(bytes, 0, bytes.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static void AuthenticationRequestHandle(string receivedData, NetworkStream stream, Encryption encryption)
    {
        try
        {
            
            // stream.Write();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}