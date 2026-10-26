namespace LineMonitor.APP.Models;

public record LineStatus
{
    public bool Running { get; init; }
    public bool Alarm { get; init; }
    public double Temperature { get; init; }  // 单位 ℃，已经除过 10
    public uint Count { get; init; }
    public DateTime Time { get; init; }        // 这次读取的时间
}