﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace AIoTComm;

public class TcpServer
{
    private static TcpServer _instance;
    private TcpListener _tcpListener;
    private NetworkStream _stream;
    
    private TcpServer()
    {
        StartServer();
    }

    public static TcpServer GetInstance()
    {
        return _instance ??= new TcpServer();
    }
    
    private async Task StartServer()
    {
        var port = 13000;
        var myIp = IPAddress.Parse("192.168.43.151");
        var hostAddress = IPAddress.Parse("127.0.0.1");
        var azureAddress = IPAddress.Parse("51.116.145.32");
        
        _tcpListener = new TcpListener(azureAddress, port);
        _tcpListener.Start();

        byte[] buffer = new byte[256];
        string receivedData;
        
        using TcpClient client = await _tcpListener.AcceptTcpClientAsync();
        Console.WriteLine("Connected!");
        
        _stream = client.GetStream();

        int readTotal;
        
        while ((readTotal = _stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            receivedData = Encoding.ASCII.GetString(buffer, 0, readTotal);
            if (receivedData == "\r\n")
            {
                Console.WriteLine("Exiting...");
            }
            else if (receivedData.Length >= 2)
            {
                IdentifyCommand(receivedData, _stream, buffer);
            }
        }
        _tcpListener.Stop();
    }
    
    private void IdentifyCommand(string receivedData, NetworkStream stream, byte[] buffer)
    {
        switch (receivedData.Substring(0,2).ToUpper())
        {
            case "TM":
                // Time request
                TimeRequestHandle(stream);
                break;
            case "MS":
                // Message response
                // handle message response
                MessageRequestHandle(receivedData, stream, buffer);
                break;
            default:
                // Unknown command
                throw new InvalidOperationException("Unknown command received.");
        }
    }
    
    private async Task TimeRequestHandle(NetworkStream stream)
    {
        try
        {
            var time = await ServiceFactory.GetClockService().GetClockTimeAsync();
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
    
    private void MessageRequestHandle(string receivedData, NetworkStream stream, byte[] buffer)
    {
        var message = receivedData.Split("|");
        var messageToSend = message[3];
        Console.WriteLine(messageToSend);
        Console.WriteLine("Message request received.");
    }
    
    public void MessageResponseHandle(string messageBody)
    {
        var message = "MS|"+ messageBody.Length +"|" + messageBody + "|";
        _stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
        Console.WriteLine("Message response received.");
    }

    public void SendMessage(string message)
    {
        throw new NotImplementedException();
    }
}