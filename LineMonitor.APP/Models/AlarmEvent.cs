namespace LineMonitor.APP.Models;

public record AlarmEvent
{
    public DateTime Time { get; init; }
    public bool IsActive { get; init; }        // true = 报警发生，false = 报警解除
    public string Message { get; init; } = "";
    public double Temperature { get; init; }
}