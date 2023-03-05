using ChatApp.IO;

namespace ChatApp;

public enum Command : byte
{
    None,
    Ping,
    String,
    ByteArray
}

/// <summary>Represents a p2p chat client agnostic to the communication protocol</summary>
public class PeerClient
{
    private readonly IByteStream byteStream; // TODO: none of these Read or Write ensure that all data has been transferred
    
    public PeerClient(IByteStream byteStream)
    {
        this.byteStream = byteStream;
    }

    public void Ping()
    {
        var commandByte = new[] { (byte)Command.Ping };
        byteStream.Write(commandByte);

        var data = new byte[32];
        new Random().NextBytes(data); // TODO: avoid this instantiation?
        byteStream.Write(data);

        while (byteStream.Read(commandByte) == 0) // TODO: this assumes that the ping returns before any other commands
        {
        }

        if (commandByte[0] != (byte)Command.Ping) throw new Exception("Command received was not a ping");

        var receivedData = new byte[32];
        byteStream.Read(receivedData);

        for (var i = 0; i < 32; i++)
        {
            if (data[i] == receivedData[i]) continue;
            Console.WriteLine("Ping data was mismatched!");
            return;
        }
        
        Console.WriteLine("Ping was successful!");
    }

    public void OnReceiveData() // TODO: this probably should be event driven
    {
        var commandByte = new byte[1];
        if (byteStream.Read(commandByte) == 0) return;

        var command = (Command)commandByte[0];

        if (command == Command.Ping)
        {
            var data = new byte[32];
            byteStream.Read(data);
            byteStream.Write(commandByte);
            byteStream.Write(data);
        } 
        else if (command == Command.String)
        {
            Console.WriteLine($"Received string: '{ByteStreamHandler.ReceiveMessage(byteStream)}'");
        }
    }
}