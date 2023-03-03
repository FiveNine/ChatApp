using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;
using System.Text;

namespace ChatApp;
public class TcpServer
{
    private IPEndPoint localEndPoint;
    
    public TcpServer()
    {
        localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 59590);
    }

    public void Listen()
    {
        Socket listener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        try
        {
            listener.Listen(10);
            Socket clientSocket = listener.Accept();
            
            Console.WriteLine("Socket connected to -> {0}", clientSocket.RemoteEndPoint.ToString());
            
            // Receive message from client
            byte[] bufferLength = new byte[1024];
            int byteReceived = clientSocket.Receive(bufferLength);
            Console.WriteLine("Message from client -> {0}",
                Encoding.ASCII.GetString(bufferLength,
                    0, byteReceived));
            
            // Send message to client
            byte[] messageSent = Encoding.ASCII.GetBytes("Greetings from Server");
            int byteSent = clientSocket.Send(messageSent);
            
            
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}