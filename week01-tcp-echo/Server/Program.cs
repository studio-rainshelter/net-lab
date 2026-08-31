using System.Net;
using System.Net.Sockets;

const int Port = 5000;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var listener = new TcpListener(IPAddress.Loopback, Port);
listener.Start();
Console.WriteLine($"Echo server listening on {IPAddress.Loopback}:{Port}. Press Ctrl+C to stop.");

try
{
    while (true)
    {
        var client = await listener.AcceptTcpClientAsync(cts.Token);
        _ = HandleClientAsync(client, cts.Token);
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Server shutting down.");
}
finally
{
    listener.Stop();
}

static async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
{
    var endpoint = client.Client.RemoteEndPoint;
    Console.WriteLine($"Client connected: {endpoint}");

    using var _ = client;
    await using var stream = client.GetStream();

    byte[] buffer = new byte[1024];

    try
    {
        while (true)
        {
            int bytesRead = await stream.ReadAsync(buffer.AsMemory(), cancellationToken);
            if (bytesRead == 0)
            {
                break; // client closed the connection
            }

            await stream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
        }
    }
    catch (OperationCanceledException)
    {
        // server is shutting down
    }
    catch (IOException)
    {
        // client disconnected abruptly
    }
    finally
    {
        Console.WriteLine($"Client disconnected: {endpoint}");
    }
}
