namespace ChatApp.IO;

// TODO: wait look at the bottom of this answer https://stackoverflow.com/a/7110437
// this interface may be redundant

/// <summary>
/// Represents a readable and writeable bidirectional byte stream.
/// Internally it will be used for testing the client by
/// making a bot client which can write to an IByteStream
/// </summary>
public interface IByteStream 
{
    /// <summary>
    /// Attempts to read 'length' bytes into a byte array at an offset
    /// </summary>
    /// <returns>The actual number of bytes read</returns>
    public int Read(byte[] bytes, int offset, int size);
    
    /// <summary>
    /// Attempts to read enough bytes from the stream to fill the byte array
    /// </summary>
    /// <returns>The actual number of bytes read</returns>
    public int Read(byte[] bytes) => Read(bytes, 0, bytes.Length);

    /// <summary>
    /// Writes 'length' bytes to the stream from the byte array at an offset
    /// </summary>
    /// /// <returns>The actual number of bytes written</returns>
    public int Write(byte[] bytes, int offset, int size);

    /// <summary>
    /// Writes the byte array to the stream
    /// </summary>
    /// <returns>The actual number of bytes written</returns>
    public int Write(byte[] bytes) => Write(bytes, 0, bytes.Length);
}