namespace Ruri.RipperHook.UnityMihoyo;
public static class MihoyoReader
{
    public static int ReadMhyInt(BinaryReader reader)
    {
        var buffer = reader.ReadBytes(6);
        return buffer[2] | (buffer[4] << 8) | (buffer[0] << 0x10) | (buffer[5] << 0x18);
    }

    public static uint ReadMhyUInt(BinaryReader reader)
    {
        var buffer = reader.ReadBytes(6);
        return (uint)(buffer[1] | (buffer[6] << 8) | (buffer[3] << 0x10) | (buffer[2] << 0x18));
    }
}

