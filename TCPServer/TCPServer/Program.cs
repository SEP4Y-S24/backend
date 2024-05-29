using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shared.Context;
using TCPServer;

class Program
{
    private static ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();
    private static readonly ClockContext _context = new ClockContext();
    private static List<Guid> _strings= new List<Guid>();
    static async Task Main(string[] args)
    {
        TcpListener listener=null;
        try
        {
            int port = 13000;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server listening on port {port}");


            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            listener?.Stop();
        }
        
    }

    private static async Task HandleClientAsync(TcpClient tcpClient)
    {
        Console.WriteLine("Client connected.");
        Client clientObj = new Client(tcpClient);
        
        using var stream = clientObj.TcpClient.GetStream();
        var buffer = new byte[1024];
        int bytesRead;
        
        try
        {
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length))!=0)
            {
                
                var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("RECEIVEDDATA: " + receivedData);
                //check if not encrypted
                if (receivedData.Substring(0, 2).ToUpper().Equals("CK"))
                {
                    // Clock Key response
                    KeyRequestHandle(receivedData, clientObj);
                }
                else if(bytesRead > 2)
                {
                    var data = buffer[..bytesRead];
                    IdentifyCommand(data, clientObj);
                }
            }
        }
        catch (IOException ex) when (ex.InnerException is SocketException socketException && socketException.SocketErrorCode == SocketError.ConnectionReset)
        {
            Console.WriteLine("Client forcibly closed the connection.");
            clients.TryRemove(clientObj.Id, out _);
            _strings.Remove(clientObj.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            Console.WriteLine("Client disconnected.");
            clients.TryRemove(clientObj.Id, out _);
            _strings.Remove(clientObj.Id);
            clientObj.TcpClient.Close();
        }

    }
    
    private static void IdentifyCommand(byte[] receivedData, Client client)
    {
        //decrypt
        // var decryptedText = client.Encryption.Decrypt(receivedData);
        
        var text = Encoding.ASCII.GetString(receivedData);
        
        //check command
        switch (text.Substring(0,2).ToUpper())
        {
            case "TM":
                // Time request
                TimeRequestHandle(client);
                break;
            case "AU":
                // Authentication response
                AuthenticationRequestHandle(text, client);
                break;
            case "MS":
                // Message response
                MessageResponseHandle(text);
                break;
            case "IR":
                // id request
                IdRequestHandle(text, client);
                break;
            default:
                // Unknown command
                throw new InvalidOperationException("Unknown command received.");
        }   
    }

    private static void SendEncrypted(string message, Client client)
    {
        var encryptedMessage = client.Encryption.Encrypt(message);
        client.TcpClient.GetStream().Write(encryptedMessage, 0, encryptedMessage.Length);
    }
    
    private static void SendMessage(byte[] message, Client client)
    {
        client.TcpClient.GetStream().Write(message, 0, message.Length);
    }
    
    private static async void TimeRequestHandle(Client client)
    {
        try
        {
            var offset=_context.Clocks.Find(client.Id).TimeOffset;
            var time = DateTime.Now.AddMinutes(offset).ToString("hh:mm");
            time= "TM|1|4|" + time.Replace(":","") + "|";
            var messageToSend = Encoding.ASCII.GetBytes(time);
            SendMessage(messageToSend,client);
            Console.WriteLine("Time request received.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Time request error: {e.Message}");
            throw;
        }
    }

    private static void IdRequestHandle(string data, Client client)
    {
        try
        {
            var message = data.Split("|");
            var id=Guid.Parse(message[1]);
            string messageToSend;
            if (_strings.Contains(id))
            {
                _strings.Remove(id);
                messageToSend = "IR|1|";
                
            }
            else
            {
                messageToSend= "IR|0|";
            }
            SendMessage(Encoding.ASCII.GetBytes(messageToSend), client);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Id request from server response error: {e.Message}");
            throw;
        }
    }
    
    
    private static void MessageResponseHandle(string decryptedText)
    {
        try
        {
            var message = decryptedText.Split("|");
            var id=Guid.Parse(message[1]);
            var messageToSend = message[2];
            Console.WriteLine($"Message {messageToSend} request received.");
            if (clients.TryGetValue(id, out Client clockClient))
            {
                messageToSend= $"MS|{messageToSend.Length}|{messageToSend}|";
                SendMessage(Encoding.ASCII.GetBytes(messageToSend), clockClient);   
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Message from server response error: {e.Message}");
            throw;
        }
        
    }
    
   private static void KeyRequestHandle(string data, Client client)
    {
        try
        {
            var message = data.Split("|");
            var receivedPublicKey = Encryption.PublicKeyFromString(message[2]);
            client.Encryption.GenerateAesKey(receivedPublicKey);
            Console.WriteLine($"Clock Key request received.");
            
            //Send SK response
            var key = $"SK|{client.Encryption.GetPublicKeyString().Length}|{client.Encryption.GetPublicKeyString()}|";
            client.TcpClient.GetStream().Write(Encoding.ASCII.GetBytes(key), 0, key.Length);
            Console.WriteLine(key);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Key request error: {e.Message}");
            throw;
        }
    }

    private static void AuthenticationRequestHandle(string receivedData, Client client)
    {
        try
        {
            var stream=client.TcpClient.GetStream();
            var message=receivedData.Split("|");
            Guid clientId;
            if (message[1].Equals("0") || _context.Clocks.Find(client.Id) == null)
            {
                
                //generate client id
                clientId= Guid.NewGuid();
                while (clients.ContainsKey(clientId))
                {
                    clientId = Guid.NewGuid();
                }
                client.Id=clientId;
                if (clients.TryAdd(clientId, client))
                {
                    _strings.Add(clientId);
                }
                
                //respond with client id
                var key = $"AU|3|{clientId.ToString().Length}|{clientId.ToString()}|";
                SendMessage(Encoding.ASCII.GetBytes(key), client);
            }
            else
            {
                //get client id
                clientId = Guid.Parse(message[2]);
                client.Id=clientId;
                if (clients.TryAdd(clientId, client))
                {
                    _strings.Add(clientId);
                }
                
                //respond with authentication code
                var messageToSend = "AU|1|0||";
                SendMessage(Encoding.ASCII.GetBytes(messageToSend), client);
            }
            Console.WriteLine($"Authentication request received for client: {clientId}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Authentication request error: {e.Message}");
            throw;
        }
    }
}