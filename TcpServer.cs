using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp;
public class TcpServer
{
    private readonly IPEndPoint localEndPoint;
    
    public TcpServer()
    {
        localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 59590);
    }

    public void Listen()
    {
        var listener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        try
        {
            listener.Listen(10);
            var clientSocket = listener.Accept();
            
            Console.WriteLine("Socket connected to -> {0}", clientSocket.RemoteEndPoint);
            
            Console.WriteLine(ReceiveMessage(clientSocket));
            
            SendMessage(clientSocket, "Greetings from Server");
            
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private static void SendMessage(Socket sock, string message)
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

    private static string ReceiveMessage(Socket sock)
    {
        var dataLengthBytes = new byte[sizeof(int)];
        sock.Receive(dataLengthBytes);
        var dataLength = BitConverter.ToInt32(dataLengthBytes, 0);
        
        var buffer = new byte[dataLength];
        var totalDataReceived = 0;
        while (totalDataReceived < dataLength)
        {
            var bytesReceived =
                sock.Receive(buffer, totalDataReceived, dataLength - totalDataReceived, SocketFlags.None);
            totalDataReceived += bytesReceived;
        }

        return Encoding.UTF8.GetString(buffer);
    }
}