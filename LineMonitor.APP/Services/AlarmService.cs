using LineMonitor.APP.Models;
using Serilog;
namespace LineMonitor.APP.Services;

public class AlarmService
{
    private bool _lastAlarm;
    private DateTime _alarmStart;

    public event Action<AlarmEvent>? AlarmChanged;

    public AlarmService(PollingService polling)
    {
        polling.StatusUpdated += OnStatusUpdated;
    }

    private void OnStatusUpdated(LineStatus s)
    {
        if (s.Alarm == _lastAlarm) return;    // 没变化，不处理
        _lastAlarm = s.Alarm;

        string message;
        if (s.Alarm)
        {
            _alarmStart = s.Time;
            message = "超温报警发生";
        }
        else
        {
            TimeSpan duration = s.Time - _alarmStart;
            message = $"超温报警解除，持续 {duration.TotalSeconds:F0} 秒";
        }

        if (s.Alarm) Log.Warning("{Message}，温度 {Temperature} ℃", message, s.Temperature);
        else Log.Information("{Message}，温度 {Temperature} ℃", message, s.Temperature);

        AlarmChanged.SafeInvoke(new AlarmEvent
        {
            Time = s.Time,
            IsActive = s.Alarm,
            Message = message,
            Temperature = s.Temperature
        });
    }
}