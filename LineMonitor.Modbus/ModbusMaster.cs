namespace LineMonitor.Modbus;

public abstract class ModbusMaster : IDisposable
{
    public byte UnitId { get; set; } = 1;
    public abstract int TimeoutMs { get; set; }
    public abstract bool IsConnected { get; }

    public abstract Task ConnectAsync();
    public abstract void Disconnect();
    public virtual void Dispose() => Disconnect();

    protected abstract Task<byte[]> SendAsync(byte[] pdu);

    public async Task<ushort[]> ReadHoldingRegistersAsync(ushort start, ushort count)
    {
        if (count < 1 || count > 125)
            throw new ArgumentOutOfRangeException(nameof(count), "一次只能读 1~125 格");

        byte[] pdu =
        {
            0x03,
            (byte)(start >> 8), (byte)start,
            (byte)(count >> 8), (byte)count
        };
        byte[] resp = await SendAsync(pdu);

        if (resp[1] != count * 2)
            throw new IOException("回答的数据长度不对");

        ushort[] values = new ushort[count];
        for (int i = 0; i < count; i++)
            values[i] = (ushort)((resp[2 + i * 2] << 8) | resp[3 + i * 2]);
        return values;
    }

    public async Task WriteSingleRegisterAsync(ushort address, ushort value)
    {
        byte[] pdu =
        {
            0x06,
            (byte)(address >> 8), (byte)address,
            (byte)(value >> 8), (byte)value
        };
        await SendAsync(pdu);
    }

    public async Task WriteMultipleRegistersAsync(ushort start, ushort[] values)
    {
        if (values.Length < 1 || values.Length > 123)
            throw new ArgumentOutOfRangeException(nameof(values), "一次只能写 1~123 格");

        byte[] pdu = new byte[6 + values.Length * 2];
        pdu[0] = 0x10;
        pdu[1] = (byte)(start >> 8);
        pdu[2] = (byte)start;
        pdu[3] = (byte)(values.Length >> 8);
        pdu[4] = (byte)values.Length;
        pdu[5] = (byte)(values.Length * 2);
        for (int i = 0; i < values.Length; i++)
        {
            pdu[6 + i * 2] = (byte)(values[i] >> 8);
            pdu[7 + i * 2] = (byte)values[i];
        }
        await SendAsync(pdu);
    }

    public async Task WriteSingleCoilAsync(ushort address, bool value)
    {
        ushort raw = value ? (ushort)0xFF00 : (ushort)0x0000;
        byte[] pdu =
        {
        0x05,
        (byte)(address >> 8), (byte)address,
        (byte)(raw >> 8), (byte)raw
    };
        await SendAsync(pdu);
    }

    public async Task<bool[]> ReadCoilsAsync(ushort start, ushort count)
    {
        if (count < 1 || count > 2000)
            throw new ArgumentOutOfRangeException(nameof(count), "一次只能读 1~2000 个线圈");

        byte[] pdu =
        {
        0x01,
        (byte)(start >> 8), (byte)start,
        (byte)(count >> 8), (byte)count
    };
        byte[] resp = await SendAsync(pdu);

        int byteCount = (count + 7) / 8;
        if (resp[1] != byteCount)
            throw new IOException("回答的数据长度不对");

        bool[] values = new bool[count];
        for (int i = 0; i < count; i++)
        {
            byte b = resp[2 + i / 8];
            values[i] = ((b >> (i % 8)) & 1) == 1;
        }
        return values;
    }
}