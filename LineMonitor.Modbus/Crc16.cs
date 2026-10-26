namespace LineMonitor.Modbus;

public static class Crc16
{
    public static ushort Compute(byte[] data, int length)
    {
        ushort crc = 0xFFFF;

        for (int i = 0; i < length; i++)
        {
            crc ^= data[i];

            for (int bit = 0; bit < 8; bit++)
            {
                if ((crc & 1) != 0)
                    crc = (ushort)((crc >> 1) ^ 0xA001);
                else
                    crc = (ushort)(crc >> 1);
            }
        }

        return crc;
    }
}