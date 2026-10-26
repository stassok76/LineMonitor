namespace LineMonitor.APP.Models;

public static class PlcAddress
{
    public const byte UnitId = 1;

    // 线圈
    public const ushort StartCmd = 0;
    public const ushort StopCmd = 1;
    public const ushort Running = 2;
    public const ushort Alarm = 3;

    // 保持寄存器
    public const ushort Temperature = 0;
    public const ushort CountHi = 1;
    public const ushort CountLo = 2;
}