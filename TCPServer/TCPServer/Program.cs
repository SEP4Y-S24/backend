using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Shared.Context;
using TCPServer;

class Program
{
    private static ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();

    private static List<Guid> _strings = new List<Guid>();
    private static ClockContext _clockContext = null;
    static async Task Main(string[] args)
    {
        Console.WriteLine(ServiceFactory.GetContext().Clocks.First(ck => ck.Id == Guid.Parse("7515d4bf-011e-4627-8a96-996e02a7ce55")).TimeOffset);
        TcpListener listener = null;
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
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {

                var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("RECEIVEDDATA: " + receivedData);
                //check if not encrypted
                if (receivedData.Substring(0, 2).ToUpper().Equals("CK"))
                {
                    // Clock Key response
                    KeyRequestHandle(receivedData, clientObj);
                }
                else if (bytesRead > 2)
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
        //Since the last version of Encryption does not work with IoT, the message sent will not be encrypted
        //decrypt \/\/\/
        //var decryptedText = client.Encryption.Decrypt(receivedData);
        //replace with the decrypted text /\/\/\
        var text = Encoding.ASCII.GetString(receivedData);

        //check command
        switch (text.Substring(0, 2).ToUpper())
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
            case "TH":
                // Time offset request
                HumidityAndTemperatureRequestHandle(text, client);
                break;
            case "IR":
                // id request
                IdRequestHandle(text, client);
                break;
            default:
                // Unknown command
                var errMsg = text.Substring(0, 3) + "|2|0||";
                SendMessage(Encoding.ASCII.GetBytes(errMsg), client);
                throw new InvalidOperationException("Unknown command received.");
        }
    }


    //Use when encryption is enabled
    private static void SendEncrypted(string message, Client client)
    {
        var encryptedMessage = client.Encryption.Encrypt(message);
        client.TcpClient.GetStream().Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    //Use when encryption is disabled
    private static void SendMessage(byte[] message, Client client)
    {
        client.TcpClient.GetStream().Write(message, 0, message.Length);
    }


    private static void HumidityAndTemperatureRequestHandle(string data, Client client)
    {
        try
        {
            string[] commands = data.Split("|");
            string[] values = commands[2].Split("-");
            double humidity = double.Parse(values[1]), temperature = double.Parse(values[3]);
            Clock clock = ServiceFactory.GetContext().Clocks.First(ck => ck.Id.Equals(client.Id));
            Measurement measurement1 = new Measurement()
            {
                ClockId = client.Id,
                Id = Guid.NewGuid(),
                TimeOfReading = DateTime.UtcNow,
                Type = MeasurementType.Humidity,
                Value = humidity
            };
            Measurement measurement2 = new Measurement()
            {
                ClockId = client.Id,
                Id = Guid.NewGuid(),
                TimeOfReading = DateTime.UtcNow,
                Type = MeasurementType.Temperature,
                Value = temperature
            };
            measurement1.Clock = clock;
            measurement2.Clock = clock;
            ServiceFactory.GetContext().Measurements.Add(measurement1);
            ServiceFactory.GetContext().Measurements.Add(measurement2);
            ServiceFactory.GetContext().SaveChanges();
            Measurement m = ServiceFactory.GetContext().Measurements.First(m => m.Id.Equals(measurement2.Id));
            Console.WriteLine(measurement1.Value + " " + measurement2.Value + " " + m.Value);
            var message = "TH|1|";
            var messageToSend = Encoding.ASCII.GetBytes(message);
            SendMessage(messageToSend, client);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private static void TimeRequestHandle(Client client)
    {
        try
        {
            Console.WriteLine(client.Id);
            //get time offset
            //Possibilities:
            //-Connect to the database and get it from there
            //-Get it from one the BackendServerClient
            //-Get it by making a request to the ClockService
            long offset = 0;    //TODO fix hardcoded value
                                // FOR DATABASE CONNECTION OPTION \/\/\/
            offset = ServiceFactory.GetContext().Clocks.FirstAsync(ck => ck.Id.Equals(client.Id)).Result.TimeOffset;
            var time = DateTime.Now;
            time = time.AddMinutes(offset);
            var message = "TM|1|4|" + time.ToString("hh:mm").Replace(":", "") + "|";
            var messageToSend = Encoding.ASCII.GetBytes(message);
            SendMessage(messageToSend, client);
            Console.WriteLine("Time request received.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Time request error: {e.Message}");
            throw;
        }
    }

    //request from BackendServerClient to confirm that the clients clock with the given id is registered
    private static void IdRequestHandle(string data, Client client)
    {
        try
        {
            var message = data.Split("|");
            var id = Guid.Parse(message[1]);
            string messageToSend;

            //try to get the client connection to confirm the id
            if (_strings.Contains(id) && clients.TryGetValue(id, out Client clockClient))
            {
                var messageToSendCk = "KV|0|";
                SendMessage(Encoding.ASCII.GetBytes(messageToSendCk), clockClient);
                _strings.Remove(id);
                messageToSend = "IR|1|";
            }
            else
            {
                messageToSend = "IR|0|";
            }
            SendMessage(Encoding.ASCII.GetBytes(messageToSend), client);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Id request from server response error: {e.Message}");
            throw;
        }
    }

    //request from BackendServerClient to send a message to the client with the given id
    private static void MessageResponseHandle(string decryptedText)
    {
        try
        {
            var message = decryptedText.Split("|");
            if (message[1].Equals("1")) return;
            var id = Guid.Parse(message[2]);
            var messageToSend = message[3];
            Console.WriteLine($"Message {messageToSend} request received.");
            if (clients.TryGetValue(id, out Client clockClient))
            {
                messageToSend = $"MS|{messageToSend.Length}|{messageToSend}|";
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
            var stream = client.TcpClient.GetStream();
            var message = receivedData.Split("|");
            Guid clientId;
            if (message[1].Equals("0") || ServiceFactory.GetContext().Clocks.First(ck => ck.Id.Equals(Guid.Parse(message[2]))) == null)
            {

                //generate client id
                clientId = Guid.NewGuid();
                while (clients.ContainsKey(clientId))
                {
                    clientId = Guid.NewGuid();
                }
                client.Id = clientId;
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
                client.Id = clientId;
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