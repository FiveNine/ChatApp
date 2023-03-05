using System.Net;
using System.Net.Sockets;
using ChatApp.IO;

namespace ChatApp;
public class TcpClient
{
    private readonly IPEndPoint localEndPoint;
    private SocketStream socketStream;

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

            socketStream = new SocketStream(socket);
            
            ByteStreamHandler.SendMessage(socketStream, "Greetings from Client");
            Console.WriteLine(ByteStreamHandler.ReceiveMessage(socketStream));
            
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}