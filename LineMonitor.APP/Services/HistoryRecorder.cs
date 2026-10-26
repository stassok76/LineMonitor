using LineMonitor.APP.Data;
using LineMonitor.APP.Models;

namespace LineMonitor.APP.Services;

public class HistoryRecorder
{
    private readonly HistoryRepository _repo;
    private DateTime _lastSaved = DateTime.MinValue;

    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);

    public HistoryRecorder(PollingService polling, HistoryRepository repo)
    {
        _repo = repo;
        polling.StatusUpdated += OnStatusUpdated;
    }

    private void OnStatusUpdated(LineStatus s)
    {
        if (s.Time - _lastSaved < Interval) return;   // 离上次保存不到 5 秒，跳过

        _lastSaved = s.Time;
        _repo.Insert(s);
    }
}