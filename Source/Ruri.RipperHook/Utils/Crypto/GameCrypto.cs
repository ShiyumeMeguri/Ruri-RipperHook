namespace Ruri.RipperHook.Crypto;

public record GameCrypto
{
    public virtual Span<byte> Decrypt(Span<byte> dataSpan) => dataSpan;
}

public record Mr0k : GameCrypto
{
    public byte[] ExpansionKey { get; }
    public byte[] SBox { get; }
    public byte[] InitVector { get; }
    public byte[] BlockKey { get; }
    public byte[] PostKey { get; }

    public Mr0k(byte[] expansionKey = null, byte[] sBox = null, byte[] initVector = null, byte[] blockKey = null, byte[] postKey = null)
    {
        ExpansionKey = expansionKey ?? Array.Empty<byte>();
        SBox = sBox ?? Array.Empty<byte>();
        InitVector = initVector ?? Array.Empty<byte>();
        BlockKey = blockKey ?? Array.Empty<byte>();
        PostKey = postKey ?? Array.Empty<byte>();
    }
}
public record GF2Xor : GameCrypto
{
    public byte[] XorKey { get; }

    public GF2Xor(byte[] key = null)
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