using System.Net.Sockets;
using System.Text;

namespace ChatApp;

public enum Command
{
    Ping,
    String,
    ByteArray
}

public static class SocketHandler
{
    private const int SendChuckSizeBytes = 1024;
    private const int MaximumBufferSizeBytes = 1024 * 1024; 

    public static void SendMessage(Socket socket, string message)
    {
        SendBytes(socket, Encoding.UTF8.GetBytes(message));
    }

    public static string ReceiveMessage(Socket socket)
    {
        return Encoding.UTF8.GetString(ReceiveBytes(socket));
    }

    /// <summary>
    /// Sends an array of bytes prefixed by the length as a four byte signed integer
    /// </summary>
    public static void SendBytes(Socket socket, byte[] bytes)
    {
        var dataLengthBytes = BitConverter.GetBytes(bytes.Length);
        
        socket.Send(dataLengthBytes);
        
        var totalDataSent = 0;
        while (totalDataSent < bytes.Length)
        {
            var bytesSent = socket.Send(bytes, totalDataSent,
                Math.Min(bytes.Length - totalDataSent, SendChuckSizeBytes), SocketFlags.None);
            totalDataSent += bytesSent;
        }
    }

    /// <summary>
    /// Receives an array of bytes prefixed by the length as a four byte signed integer
    /// </summary>
    /// <returns>The received byte array</returns>
    public static byte[] ReceiveBytes(Socket socket)
    {
        var dataLengthBytes = new byte[4];
        socket.Receive(dataLengthBytes);
        var dataLength = BitConverter.ToInt32(dataLengthBytes, 0);

        if (dataLength > MaximumBufferSizeBytes)
        {
            throw new Exception(
                $"Tried to receive message of length {dataLength} bytes, maximum is {MaximumBufferSizeBytes}");
        }
        
        var buffer = new byte[dataLength]; 
        
        var totalDataReceived = 0;
        while (totalDataReceived < dataLength)
        {
            var bytesReceived =
                socket.Receive(buffer, totalDataReceived, dataLength - totalDataReceived, SocketFlags.None);
            totalDataReceived += bytesReceived;
        }

        return buffer;
    }
}