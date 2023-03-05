using System.Net;

namespace ChatApp;

public abstract class Program
{
    private static Dictionary<string, string> LoadConfigVariables(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Could not find environment file '{filePath}'");

        var dict = new Dictionary<string, string>();
        
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('=');
            dict.Add(parts[0], parts[1]);
        }

        return dict;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var configVarDict = LoadConfigVariables("../../../.env");

        var peerAEndPoint = new IPEndPoint(
            IPAddress.Parse(configVarDict["PEER_A_IP"]),
            int.Parse(configVarDict["PEER_A_PORT"])
        );

        var peerBEndPoint = new IPEndPoint(
            IPAddress.Parse(configVarDict["PEER_B_IP"]),
            int.Parse(configVarDict["PEER_B_PORT"])
        );

        switch (args[0])
        {
            case "c":
                var client = new TcpClient();
                client.Connect(
                    new IPEndPoint(
                        IPAddress.Parse(configVarDict["SERVER_IP"]),
                        int.Parse(configVarDict["SERVER_PORT"])
                    )
                );
                break;
            case "s":
                var server = new TcpServer();
                server.Listen();
                break;
            case "a":
            {
                var peer = new TcpPeerClient(peerAEndPoint, "Alice");
                peer.Connect(peerBEndPoint);
            } break;
            case "b":
            {
                var peer = new TcpPeerClient(peerBEndPoint, "Bob");
                peer.Connect(peerAEndPoint);
            } break;
        }
    }
}