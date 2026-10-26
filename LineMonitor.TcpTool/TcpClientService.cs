using System.Net.Sockets;

namespace LineMonitor.TcpTool;

public class TcpClientService
{
    private TcpClient? _client;
    private CancellationTokenSource? _cts;
    private string _host = "";
    private int _port;

    public bool IsConnected => _client != null;
    public bool IsActive => _cts != null && !_cts.IsCancellationRequested;

    public bool AutoReconnect { get; set; }
    public int ReconnectSeconds { get; set; } = 3;

    public event Action<string>? Disconnected;
    public event Action<byte[]>? DataReceived;
    public event Action<int>? Reconnecting;   // 参数：第几次尝试
    public event Action? Reconnected;

    public async Task ConnectAsync(string host, int port)
    {
        if (IsActive) return;

        _host = host;
        _port = port;
        _cts = new CancellationTokenSource();

        try
        {
            await OpenAsync(_cts.Token);
        }
        catch
        {
            _cts.Cancel();
            throw;
        }
    }

    private async Task OpenAsync(CancellationToken token)
    {
        var client = new TcpClient();
        try
        {
            await client.ConnectAsync(_host, _port, token);
        }
        catch
        {
            client.Dispose();
            throw;
        }

        _client = client;
        _ = ReceiveLoopAsync(client, token);
    }
    private async Task ReceiveLoopAsync(TcpClient client, CancellationToken token)
    {
        string reason = "服务器关闭了连接";
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int n = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (n == 0) break;
                DataReceived?.Invoke(buffer[..n]);
            }
        }
        catch (OperationCanceledException)
        {
            reason = "主动断开";
        }
        catch (IOException ex)
        {
            reason = "异常断开：" + ex.Message;
        }
        finally
        {
            client.Dispose();
            if (_client == client) _client = null;
            Disconnected?.Invoke(reason);

            if (AutoReconnect && !token.IsCancellationRequested)
                _ = ReconnectLoopAsync(token);
        }
    }
    private async Task ReconnectLoopAsync(CancellationToken token)
    {
        int attempt = 0;
        while (!token.IsCancellationRequested)
        {
            attempt++;
            Reconnecting?.Invoke(attempt);
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(ReconnectSeconds), token);
                await OpenAsync(token);
                Reconnected?.Invoke();
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (SocketException)
            {
                // 还是连不上，进入下一轮
            }
        }
    }
    public async Task SendAsync(byte[] data)
    {
        if (_client == null) throw new InvalidOperationException("未连接");
        await _client.GetStream().WriteAsync(data);
    }

    public void Disconnect()
    {
        _cts?.Cancel();
    }
}