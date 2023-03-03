using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp;
public class TcpClient
{
    private readonly IPEndPoint localEndPoint;

    public TcpClient()
    {
        localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 59591);
    }

    public void Connect(IPEndPoint endPoint)
    {
        var sock = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        sock.Bind(localEndPoint);
        try
        {
            sock.Connect(endPoint);
            Console.WriteLine("Socket connected to -> {0}", sock.RemoteEndPoint);
            
            SendMessage(sock, "Greetings from Client");
            
            Console.WriteLine(ReceiveMessage(sock));
            
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private static void SendMessage(Socket sock, String message)
    {
        var rawData = Encoding.UTF8.GetBytes(message);
        var dataLength = BitConverter.GetBytes(rawData.Length);
        sock.Send(dataLength);
        
        var totalDataSent = 0;
        while (totalDataSent < rawData.Length)
        {
            var bytesSent = sock.Send(rawData, totalDataSent, Math.Min(rawData.Length - totalDataSent, 1024), SocketFlags.None);
            totalDataSent += bytesSent;
        }
        
    }

    private static String ReceiveMessage(Socket sock)
    {
        var dataLengthBytes = new byte[sizeof(int)];
        sock.Receive(dataLengthBytes);
        var dataLength = BitConverter.ToInt32(dataLengthBytes, 0);
        
        var buffer = new byte[dataLength];
        var totalDataReceived = 0;
        while (totalDataReceived < dataLength)
        {
            int bytesReceived =
                sock.Receive(buffer, totalDataReceived, dataLength - totalDataReceived, SocketFlags.None);
            totalDataReceived += bytesReceived;
        }

        return Encoding.UTF8.GetString(buffer);
    }
}