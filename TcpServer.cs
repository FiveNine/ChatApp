using System.Net;
using System.Net.Sockets;

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

            Console.WriteLine(SocketHandler.ReceiveMessage(clientSocket));
            
            SocketHandler.SendMessage(clientSocket, "Greetings from Server");
            
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}