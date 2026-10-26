namespace LineMonitor.Modbus;

public class ModbusException : Exception
{
    public byte ErrorCode { get; }

    public ModbusException(byte errorCode)
        : base($"PLC 返回错误，错误码 {errorCode}（{Describe(errorCode)}）")
    {
        ErrorCode = errorCode;
    }

    private static string Describe(byte code) => code switch
    {
        1 => "非法功能码",
        2 => "非法数据地址",
        3 => "非法数据值",
        4 => "从站设备故障",
        _ => "未知错误"
    };
}