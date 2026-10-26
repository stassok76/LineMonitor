using System.Net.Sockets;

namespace LineMonitor.Modbus;

public class ModbusTcpMaster : ModbusMaster
{
    private readonly string _host;
    private readonly int _port;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private ushort _transactionId = 0;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public override int TimeoutMs { get; set; } = 1000;
    public override bool IsConnected => _client != null;

    public ModbusTcpMaster(string host, int port = 502)
    {
        _host = host;
        _port = port;
    }

    public override async Task ConnectAsync()
    {
        if (IsConnected) return;
        var client = new TcpClient();
        try
        {
            using var cts = new CancellationTokenSource(TimeoutMs);
            await client.ConnectAsync(_host, _port, cts.Token);
        }
        catch (OperationCanceledException)      // ← 新增：超时
        {
            client.Dispose();
            throw new TimeoutException($"连接 PLC 超时（{TimeoutMs} 毫秒）");
        }
        catch
        {
            client.Dispose();

            throw;
        }
        _client = client;
        _stream = client.GetStream();
    }

    public override void Disconnect()
    {
        _stream = null;
        _client?.Dispose();
        _client = null;
    }

    private byte[] BuildFrame(byte[] pdu)
    {
        int length = pdu.Length + 1;
        byte[] frame = new byte[6 + length];

        frame[0] = (byte)(_transactionId >> 8);
        frame[1] = (byte)_transactionId;
        frame[2] = 0;
        frame[3] = 0;
        frame[4] = (byte)(length >> 8);
        frame[5] = (byte)length;
        frame[6] = UnitId;
        pdu.CopyTo(frame, 7);

        return frame;
    }

    private async Task<byte[]> ReadExactAsync(int length, CancellationToken token)
    {
        byte[] buffer = new byte[length];
        int got = 0;
        while (got < length)
        {
            int n = await _stream!.ReadAsync(buffer, got, length - got, token);
            if (n == 0) throw new IOException("连接被关闭");
            got += n;
        }
        return buffer;
    }

    protected override async Task<byte[]> SendAsync(byte[] pdu)
    {
        if (_stream == null) throw new InvalidOperationException("未连接");

        await _lock.WaitAsync();
        try
        {
            _transactionId++;
            byte[] request = BuildFrame(pdu);
            byte[] header, body;

            using var cts = new CancellationTokenSource(TimeoutMs);
            try
            {
                await _stream.WriteAsync(request, cts.Token);
                header = await ReadExactAsync(6, cts.Token);
                int length = (header[4] << 8) | header[5];
                body = await ReadExactAsync(length, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Disconnect();
                throw new TimeoutException($"PLC 超过 {TimeoutMs} 毫秒没有回答");
            }
            catch (IOException)          // ← 新增
            {
                Disconnect();
                throw;
            }

            if (header[0] != request[0] || header[1] != request[1])
            {
                Disconnect();
                throw new IOException("问题编号对不上");
            }

            byte[] responsePdu = body[1..];
            if (responsePdu[0] == pdu[0] + 0x80)
                throw new ModbusException(responsePdu[1]);

            return responsePdu;
        }
        finally
        {
            _lock.Release();
        }
    }
   
}