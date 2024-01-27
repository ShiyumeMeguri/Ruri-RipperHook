namespace Ruri.RipperHook.Crypto;

public record GameCrypto
{
}

public record Mr0k : GameCrypto
{
    public Mr0k(byte[] expansionKey = null,
        byte[] sBox = null,
        byte[] initVector = null,
        byte[] blockKey = null,
        byte[] postKey = null)
    {
        ExpansionKey = expansionKey ?? Array.Empty<byte>();
        SBox = sBox ?? Array.Empty<byte>();
        InitVector = initVector ?? Array.Empty<byte>();
        BlockKey = blockKey ?? Array.Empty<byte>();
        PostKey = postKey ?? Array.Empty<byte>();
    }

    public byte[] ExpansionKey { get; }
    public byte[] SBox { get; }
    public byte[] InitVector { get; }
    public byte[] BlockKey { get; }
    public byte[] PostKey { get; }
}