using LineMonitor.Modbus;

ModbusMaster plc = new ModbusTcpMaster("127.0.0.1");

using (plc)
{
    await plc.ConnectAsync();
    Console.WriteLine("已连接 PLC");

    await plc.WriteSingleCoilAsync(0, true);
    Console.WriteLine("已按下启动");

    for (int t = 0; t < 10; t++)
    {
        bool[] coils = await plc.ReadCoilsAsync(0, 4);
        ushort[] regs = await plc.ReadHoldingRegistersAsync(0, 3);

        double temp = regs[0] / 10.0;
        int count = (regs[1] << 16) | regs[2];

        Console.WriteLine($"运行={coils[2]} 报警={coils[3]} 温度={temp}℃ 产量={count}");
        await Task.Delay(1000);
    }

    await plc.WriteSingleCoilAsync(1, true);
    Console.WriteLine("已按下停止");
}