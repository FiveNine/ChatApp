using System.Net;
using System.Net.Sockets;
using ChatApp.IO;

namespace ChatApp;
public class TcpClient
{
    private readonly IPEndPoint localEndPoint = new(IPAddress.Parse("0.0.0.0"), 59591);
    private SocketStream socketStream;

    public void Connect(IPEndPoint endPoint)
    {
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(localEndPoint);
        try
        {
            socket.Connect(endPoint);
            Console.WriteLine("Socket connected to -> {0}", socket.RemoteEndPoint);

            socketStream = new SocketStream(socket);
            while (true)
            {
                var input = Console.ReadLine();
                ByteStreamHandler.SendMessage(socketStream, input);
                if (input == "QUIT!") break;
                var receivedMessage = ByteStreamHandler.ReceiveMessage(socketStream);
                Console.WriteLine(receivedMessage);
            }
            
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}