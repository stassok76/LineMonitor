using System.Text;

namespace LineMonitor.TcpTool;

public static class HexHelper
{
    public static byte[] ToBytes(string input, bool isHex)
    {
        if (isHex) return Convert.FromHexString(input.Replace(" ", ""));
        return Encoding.UTF8.GetBytes(input);
    }

    public static string ToText(byte[] data, bool isHex)
    {
        if (isHex) return BitConverter.ToString(data).Replace("-", " ");
        return Encoding.UTF8.GetString(data);
    }
}