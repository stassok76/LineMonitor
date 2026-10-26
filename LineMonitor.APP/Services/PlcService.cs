using LineMonitor.APP.Models;
using LineMonitor.Modbus;

namespace LineMonitor.APP.Services;

public class PlcService
{
    private readonly ModbusMaster _master;

    public PlcService(ModbusMaster master)
    {
        _master = master;
    }
    public async Task<LineStatus> ReadStatusAsync()
    {
        // 线圈 2、3 → coils[0] = running, coils[1] = alarm
        bool[] coils = await _master.ReadCoilsAsync(PlcAddress.Running, 2);

        // 寄存器 0、1、2 → 温度、产量高位、产量低位
        ushort[] regs = await _master.ReadHoldingRegistersAsync(PlcAddress.Temperature, 3);

        return new LineStatus
        {
            Running = coils[0],
            Alarm = coils[1],
            Temperature = (short)regs[0] / 10.0,
            Count = ((uint)regs[1] << 16) | regs[2],
            Time = DateTime.Now
        };
    }

    public Task StartAsync() => _master.WriteSingleCoilAsync(PlcAddress.StartCmd, true);

    public Task StopAsync() => _master.WriteSingleCoilAsync(PlcAddress.StopCmd, true);

    public bool IsConnected => _master.IsConnected;

    public Task ConnectAsync() => _master.ConnectAsync();
}