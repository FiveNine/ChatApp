using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;
using System.Text;

namespace ChatApp;
public class TcpClient
{
    private IPEndPoint _localEndPoint;

    public TcpClient()
    {
        _localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 59591);
    }

    public void Connect(IPEndPoint endPoint)
    {
        Socket sock = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        sock.Bind(_localEndPoint);
        try
        {
            sock.Connect(endPoint);
            Console.WriteLine("Socket connected to -> {0}", sock.RemoteEndPoint?.ToString());
            
            //Send message to server
            byte[] messageSent = Encoding.ASCII.GetBytes("Greetings from Client");
            int byteSent = sock.Send(messageSent);
            
            //Receive message from server
            byte[] bufferLength = new byte[1024];
            int byteReceived = sock.Receive(bufferLength);
            Console.WriteLine("Message from Server -> {0}",
                Encoding.ASCII.GetString(bufferLength,
                    0, byteReceived));
            
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}