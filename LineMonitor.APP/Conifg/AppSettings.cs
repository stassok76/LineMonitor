namespace LineMonitor.APP.Config;

public class AppSettings
{
    public PlcSettings Plc { get; set; } = new();
    public int PollIntervalMs { get; set; } = 500;
    public int HistoryIntervalSeconds { get; set; } = 5;
    public string DatabaseFile { get; set; } = "linemonitor.db";
}

public class PlcSettings
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 502;
    public byte UnitId { get; set; } = 1;
    public int TimeoutMs { get; set; } = 1000;
}