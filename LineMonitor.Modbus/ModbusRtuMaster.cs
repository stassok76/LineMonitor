using System.IO.Ports;

namespace LineMonitor.Modbus;

public class ModbusRtuMaster : ModbusMaster
{
    private readonly SerialPort _port;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public override bool IsConnected => _port.IsOpen;

    public override int TimeoutMs
    {
        get => _port.ReadTimeout;
        set => _port.ReadTimeout = value;
    }

    public ModbusRtuMaster(string portName, int baudRate = 9600)
    {
        _port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        _port.ReadTimeout = 1000;
    }

    public override Task ConnectAsync()
    {
        _port.Open();
        return Task.CompletedTask;
    }
    public override void Disconnect() => _port.Close();
    public override void Dispose() => _port.Dispose();
    private byte[] BuildFrame(byte[] pdu)
    {
        byte[] frame = new byte[1 + pdu.Length + 2];
        frame[0] = UnitId;
        pdu.CopyTo(frame, 1);

        ushort crc = Crc16.Compute(frame, frame.Length - 2);
        frame[^2] = (byte)crc;
        frame[^1] = (byte)(crc >> 8);

        return frame;
    }
    private byte[] ReadExact(int length)
    {
        byte[] buffer = new byte[length];
        int got = 0;
        while (got < length)
            got += _port.Read(buffer, got, length - got);
        return buffer;
    }

    private byte[] Transact(byte[] pdu)
    {
        byte[] request = BuildFrame(pdu);
        _port.DiscardInBuffer();
        _port.Write(request, 0, request.Length);
        Console.WriteLine("发送：" + BitConverter.ToString(request).Replace("-", " "));

        var resp = new List<byte>();
        resp.AddRange(ReadExact(2));
        byte fc = resp[1];

        if (fc == pdu[0] + 0x80)
        {
            resp.AddRange(ReadExact(3));
        }
        else if (fc == 0x03)
        {
            resp.AddRange(ReadExact(1));
            resp.AddRange(ReadExact(resp[2] + 2));
        }
        else if (fc == 0x06 || fc==0x10)
        {
            resp.AddRange(ReadExact(6));
        }
        else
        {
            throw new IOException($"不支持的功能码 {fc:X2}");
        }

        byte[] frame = resp.ToArray();
        Console.WriteLine("收到：" + BitConverter.ToString(frame).Replace("-", " "));

        ushort crc = Crc16.Compute(frame, frame.Length - 2);
        if (frame[^2] != (byte)crc || frame[^1] != (byte)(crc >> 8))
            throw new IOException("CRC 校验失败，数据可能被干扰");

        if (frame[0] != UnitId)
            throw new IOException("回答的 PLC 号不对");

        byte[] responsePdu = frame[1..^2];
        if (responsePdu[0] == pdu[0] + 0x80)
            throw new ModbusException(responsePdu[1]);

        return responsePdu;
    }
    protected override async Task<byte[]> SendAsync(byte[] pdu)
    {
        if (!_port.IsOpen) throw new InvalidOperationException("串口未打开");

        await _lock.WaitAsync();
        try
        {
            return await Task.Run(() => Transact(pdu));
        }
        finally
        {
            _lock.Release();
        }
    }

}