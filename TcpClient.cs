using System.Net;
using System.Net.Sockets;

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
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(localEndPoint);
        try
        {
            socket.Connect(endPoint);
            Console.WriteLine("Socket connected to -> {0}", socket.RemoteEndPoint);
            
            SocketHandler.SendMessage(socket, "Greetings from Client");
            
            Console.WriteLine(SocketHandler.ReceiveMessage(socket));
            
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}