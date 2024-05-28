using System.Net.Sockets;
using System.Text;

namespace TCPClient;

public class TCPClient
{
    public TCPClient()
    {
    }

    private async void StartServer()
    {
        try
        {
            string serverAddress = "";
            int serverPort = 0;
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(serverAddress,serverPort);
                using (var networkStream = client.GetStream())
                {
                    // Send message to the server
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await networkStream.WriteAsync(messageBytes, 0, messageBytes.Length);

                    // Wait for the server response
                    byte[] buffer = new byte[1024];
                    int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    return jsonResponse;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (logging, rethrowing, etc.)
            throw new InvalidOperationException("An error occurred while communicating with the socket server.", ex);
        }
    }
}