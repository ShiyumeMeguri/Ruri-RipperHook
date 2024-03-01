using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook.GirlsFrontline2Common;

public record XorDecryptor : CommonDecryptor
{
    public byte[] XorKey { get; }

    public XorDecryptor(byte[] key = null)
    {
        XorKey = key ?? Array.Empty<byte>();
    }

    private Span<byte> XorBuffer(Span<byte> buffer, ReadOnlySpan<byte> key)
    {
        int size = Math.Min(buffer.Length, key.Length);
        Span<byte> result = new byte[size];
        for (int i = 0; i < size; i++)
        {
            result[i] = (byte)(buffer[i] ^ key[i]);
        }
        return result;
    }

    public override Span<byte> Decrypt(Span<byte> data)
    {
        Span<byte> key = XorBuffer(data, XorKey);
        for (int i = 0; i < Math.Min(0x1000 * 8, data.Length); i++)
        {
            data[i] = (byte)(data[i] ^ key[i % key.Length]);
        }
        return data;
    }
}