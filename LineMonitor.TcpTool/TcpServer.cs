using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace LineMonitor.TcpTool;

public class TcpServer
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<string, TcpClient> _clients = new();

    public bool IsRunning => _listener != null;
    public int TimeoutSeconds { get; set; } = 0;   // 0 表示不检测超时

    public event Action<string>? ClientConnected;            // 参数：客户端 id
    public event Action<string, string>? ClientDisconnected; // 参数：id、断开原因
    public event Action<string, byte[]>? DataReceived;       // 参数：id、收到的字节
    public void Start(int port)
    {
        if (IsRunning) return;
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        _cts = new CancellationTokenSource();
        _ = AcceptLoopAsync(_cts.Token);
    }

    private async Task AcceptLoopAsync(CancellationToken token)
    {
        try
        {
            while (true)
            {
                TcpClient client = await _listener!.AcceptTcpClientAsync(token);
                string id = client.Client.RemoteEndPoint!.ToString()!;
                _clients[id] = client;
                ClientConnected?.Invoke(id);
                _ = HandleClientAsync(id, client, token);
            }
        }
        catch when (token.IsCancellationRequested)
        {
            // 停止服务器时会走到这里，属于正常退出
        }
    }
    private async Task HandleClientAsync(string id, TcpClient client, CancellationToken stopToken)
    {
        string reason = "对方正常断开";
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(stopToken);
                if (TimeoutSeconds > 0)
                    cts.CancelAfter(TimeSpan.FromSeconds(TimeoutSeconds));

                int n = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                if (n == 0) break;

                DataReceived?.Invoke(id, buffer[..n]);
            }
        }
        catch (OperationCanceledException)
        {
            reason = stopToken.IsCancellationRequested
                ? "服务器已停止"
                : $"超过 {TimeoutSeconds} 秒无数据";
        }
        catch (IOException ex)
        {
            reason = "异常断开：" + ex.Message;
        }
        finally
        {
            _clients.TryRemove(id, out _);
            client.Dispose();
            ClientDisconnected?.Invoke(id, reason);
        }
    }
    public async Task SendAsync(string id, byte[] data)
    {
        if (_clients.TryGetValue(id, out var client))
            await client.GetStream().WriteAsync(data);
    }

    public async Task BroadcastAsync(byte[] data)
    {
        foreach (var client in _clients.Values)
        {
            try { await client.GetStream().WriteAsync(data); }
            catch { }  // 发失败的客户端，它自己的读取循环会发现并移除
        }
    }

    public void Stop()
    {
        if (!IsRunning) return;
        _cts!.Cancel();
        _listener!.Stop();
        _listener = null;
    }
}