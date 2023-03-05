using System.Net.Sockets;

namespace ChatApp.IO;

public class SocketStream: IByteStream
{
    private readonly Socket socket;

    public SocketStream(Socket socket)
    {
        this.socket = socket;
    }
    
    public int Read(byte[] bytes, int offset, int size)
    {
        return socket.Receive(bytes, offset, size, SocketFlags.None);
    }

    public int Write(byte[] bytes, int offset, int size)
    {
        return socket.Send(bytes, offset, size, SocketFlags.None);
    }
}