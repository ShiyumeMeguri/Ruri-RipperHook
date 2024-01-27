using System.Buffers.Binary;

namespace Ruri.RipperHook.Crypto;

public static class XORShift128
{
    private const long SEED = 0x61C8864E7A143579;
    private const uint MT19937 = 0x6C078965;
    private static uint x, y, z, w, initseed;

    public static bool Init;

    public static void InitSeed(uint seed)
    {
        initseed = seed;
        x = seed;
        y = MT19937 * x + 1;
        z = MT19937 * y + 1;
        w = MT19937 * z + 1;

        Init = true;
    }

    public static uint XORShift()
    {
        var t = x ^ (x << 11);
        x = y;
        y = z;
        z = w;
        return w = w ^ (w >> 19) ^ t ^ (t >> 8);
    }

    public static uint NextUInt32()
    {
        return XORShift();
    }

    public static int NextDecryptInt()
    {
        return BinaryPrimitives.ReadInt32LittleEndian(NextDecrypt(4));
    }

    public static uint NextDecryptUInt()
    {
        return BinaryPrimitives.ReadUInt32LittleEndian(NextDecrypt(4));
    }

    public static long NextDecryptLong()
    {
        return BinaryPrimitives.ReadInt64LittleEndian(NextDecrypt(8));
    }

    public static byte[] NextDecrypt(int size)
    {
        var valueBytes = new byte[size];
        var key = size * initseed - SEED;
        var keyBytes = BitConverter.GetBytes(key);
        for (var i = 0; i < size; i++)
        {
            var val = NextUInt32();
            valueBytes[i] = keyBytes[val % 8];
        }

        return valueBytes;
    }
}