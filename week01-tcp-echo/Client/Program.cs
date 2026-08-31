using System.Net.Sockets;
using System.Text;

const string Host = "127.0.0.1";
const int Port = 5000;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

using var client = new TcpClient();
await client.ConnectAsync(Host, Port, cts.Token);
await using var stream = client.GetStream();

Console.WriteLine($"Connected to {Host}:{Port}. Type a message and press Enter (Ctrl+C to quit).");

byte[] buffer = new byte[1024];

try
{
    while (true)
    {
        string? line = await Console.In.ReadLineAsync(cts.Token);
        if (line is null)
        {
            break;
        }

        int byteCount = Encoding.UTF8.GetBytes(line, buffer);
        await stream.WriteAsync(buffer.AsMemory(0, byteCount), cts.Token);

        int bytesRead = await stream.ReadAsync(buffer.AsMemory(), cts.Token);
        if (bytesRead == 0)
        {
            Console.WriteLine("Server closed the connection.");
            break;
        }

        string echoed = Encoding.UTF8.GetString(buffer.AsSpan(0, bytesRead));
        Console.WriteLine($"Echo: {echoed}");
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Client shutting down.");
}
