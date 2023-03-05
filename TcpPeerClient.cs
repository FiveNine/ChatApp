using System.Net;
using System.Net.Sockets;
using ChatApp.IO;

namespace ChatApp;

public class TcpPeerClient
{
    private readonly IPEndPoint localEndPoint;
    private readonly string name;

    public TcpPeerClient(IPEndPoint localEndPoint)
    {
        this.localEndPoint = localEndPoint;
    }
    
    public TcpPeerClient(IPEndPoint localEndPoint, string name)
    {
        this.localEndPoint = localEndPoint;
        this.name = name;
    }

    /// <summary>
    /// Attempts to connect to the endpoint first. If this fails, the client listens for a connection
    /// </summary>
    public void Connect(IPEndPoint endPoint)
    {
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(localEndPoint);
        
        Socket peerSocket;

        try
        {
            socket.Connect(endPoint);
            peerSocket = socket;
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Socket failed to connect, peer is not listening!");
            Console.WriteLine("Listening for connection instead...");
            socket.Listen(10);
            peerSocket = socket.Accept();
            socket.Close();
        }

        Console.WriteLine("Socket connected to -> {0}", peerSocket.RemoteEndPoint);

        var socketStream = new SocketStream(peerSocket);

        ByteStreamHandler.SendMessage(socketStream, $"Greetings from {name}!");
        Console.WriteLine($"Received message: {ByteStreamHandler.ReceiveMessage(socketStream)}");
        
        peerSocket.Shutdown(SocketShutdown.Both);
        peerSocket.Close();
    }
}