using System.Text;

namespace ChatApp.IO;

public static class ByteStreamHandler
{
    
    private const int SendChuckSizeBytes = 1024;
    private const int MaximumBufferSizeBytes = 1024 * 1024;
    
    
    public static void SendMessage(IByteStream byteStream, string message)
    {
        WriteBytes(byteStream, Encoding.UTF8.GetBytes(message));
    }

    public static string ReceiveMessage(IByteStream byteStream)
    {
        return Encoding.UTF8.GetString(ReadBytes(byteStream));
    }

    /// <summary>
    /// Sends an array of bytes prefixed by the length as a four byte signed integer
    /// </summary>
    public static void WriteBytes(IByteStream byteStream, byte[] bytes)
    {
        var dataLengthBytes = BitConverter.GetBytes(bytes.Length);
        
        byteStream.Write(dataLengthBytes);
        
        var totalDataSent = 0;
        while (totalDataSent < bytes.Length)
        {
            var bytesSent = byteStream.Write(bytes, totalDataSent,
                Math.Min(bytes.Length - totalDataSent, SendChuckSizeBytes));
            totalDataSent += bytesSent;
        }
    }

    /// <summary>
    /// Receives an array of bytes prefixed by the length as a four byte signed integer
    /// </summary>
    /// <returns>The received byte array</returns>
    public static byte[] ReadBytes(IByteStream byteStream)
    {
        var dataLengthBytes = new byte[4];
        byteStream.Read(dataLengthBytes);
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
                byteStream.Read(buffer, totalDataReceived, dataLength - totalDataReceived);
            totalDataReceived += bytesReceived;
        }

        return buffer;
    }
}