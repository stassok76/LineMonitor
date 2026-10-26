using LineMonitor.APP.Models;
using Serilog;
namespace LineMonitor.APP.Services;

public class PollingService
{
    private readonly PlcService _plc;

    private CancellationTokenSource? _cts;

    private Task? _loopTask;

    private bool _lastConnected;
    private string? _lastError;

    public event Action<LineStatus>? StatusUpdated;
    public event Action<bool>? ConnectionChanged;
    public event Action<string>? ErrorOccurred;

    public int IntervalMs { get; set; } = 500;

    public PollingService(PlcService plc)
    {
        _plc = plc;
    }
    public void Start()
    {
        if (_cts != null) return;          // 已经在跑了，不重复启动
        _cts = new CancellationTokenSource();
        _loopTask = Task.Run(() => LoopAsync(_cts.Token));
    }

    public void Stop()
    {
        if (_cts == null) return;

        _cts.Cancel();
        try
        {
            _loopTask?.Wait(TimeSpan.FromSeconds(2));           // ← 最多等 2 秒，让循环自己结束
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "停止轮询时出错");
        }

        _cts = null;
        _loopTask = null;
        Log.Information("轮询已停止");
    }

    private async Task LoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await PollOnceAsync();

            try
            {
                await Task.Delay(IntervalMs, token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
    private async Task PollOnceAsync()
    {
        try
        {
            if (!_plc.IsConnected)
                await _plc.ConnectAsync();

            LineStatus status = await _plc.ReadStatusAsync();
            SetConnected(true);
            _lastError = null;
            StatusUpdated.SafeInvoke(status);
        }
        catch (Exception ex)
        {
            SetConnected(_plc.IsConnected);
            if (ex.Message != _lastError)            // 和上次的错误不同才记录
            {
                Log.Warning("轮询出错：{Error}", ex.Message);
                _lastError = ex.Message;
            }
            ErrorOccurred?.Invoke(ex.Message);
        }
    }

    private void SetConnected(bool connected)
    {
        if (connected == _lastConnected) return;
        _lastConnected = connected;

        if (connected) Log.Information("PLC 已连接");
        else Log.Warning("PLC 连接断开");

        ConnectionChanged?.Invoke(connected);
    }
}