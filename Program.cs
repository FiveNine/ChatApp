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
        
        Console.WriteLine($"Client IP is {configVarDict["CLIENT_IP"]}");
        Console.WriteLine($"Peer IP is {configVarDict["PEER_IP"]}");
    }
}