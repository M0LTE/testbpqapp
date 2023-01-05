using bpqapplib;
using System.Net;
using System.Net.Sockets;

var tcpListener = new TcpListener(IPAddress.Loopback, 63000);
tcpListener.Start();

Console.WriteLine("Listening on port 63000");

while (true)
{
    var tcpClient = await tcpListener.AcceptTcpClientAsync();
    _ = Task.Run(() => HandleClient(tcpClient));
}

static async Task HandleClient(TcpClient tcpClient)
{
    try
    {
        using var stream = tcpClient.GetStream();
        using var streamReader = new StreamReader(stream);
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };

        var fromcall = await streamReader.ReadLineAsync();

        Console.WriteLine($"Accepted a connection from {fromcall}");

        await streamWriter.WriteLineAsync($"Hello {fromcall} what is your name?");

        var name = await streamReader.ReadLineAsync();

        await streamWriter.WriteLineAsync($"Nice to meet you {name}, bye for now");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex}");
    }
    finally
    {
        tcpClient.Dispose();
    }
}