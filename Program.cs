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
        
        switch (args[0])
        {
            case "c":
                TcpClient client = new TcpClient();
                client.Connect(
                    new IPEndPoint(
                        IPAddress.Parse(configVarDict["SERVER_IP"]), 
                        int.Parse(configVarDict["SERVER_PORT"])
                    )
                );
                break;
            case "s":
                TcpServer server = new TcpServer();
                server.Listen();
                break;
        }
        
    }
}