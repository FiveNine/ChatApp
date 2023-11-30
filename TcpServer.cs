using System.Net;
using System.Net.Sockets;
using ChatApp.IO;

namespace ChatApp;
public class TcpServer
{
    private readonly IPEndPoint localEndPoint;
    private SocketStream socketStream;
    
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

            socketStream = new SocketStream(clientSocket);

            while (true)
            {
                var receivedMessage = ByteStreamHandler.ReceiveMessage(socketStream);
                Console.WriteLine(receivedMessage);
                if (receivedMessage == "QUIT!") break;
                ByteStreamHandler.SendMessage(socketStream, $"Echo: {receivedMessage}");
            }
            
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}