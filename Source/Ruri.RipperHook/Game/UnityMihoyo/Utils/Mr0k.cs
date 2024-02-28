using AssetRipper.SourceGenerated.Extensions;
using Ruri.RipperHook;
using Ruri.RipperHook.Crypto;
using System.Buffers.Binary;

public record Mr0k : CommonDecryptor
{
    public byte[] ExpansionKey { get; }
    public byte[] SBox { get; }
    public byte[] InitVector { get; }
    public byte[] BlockKey { get; }
    public byte[] PostKey { get; }

    private static readonly int BlockSize = 0x400;

    private static readonly byte[] mr0kMagic = { 0x6D, 0x72, 0x30, 0x6B };

    public Mr0k(byte[] expansionKey = null, byte[] sBox = null, byte[] initVector = null, byte[] blockKey = null, byte[] postKey = null)
    {
        ExpansionKey = expansionKey ?? Array.Empty<byte>();
        SBox = sBox ?? Array.Empty<byte>();
        InitVector = initVector ?? Array.Empty<byte>();
        BlockKey = blockKey ?? Array.Empty<byte>();
        PostKey = postKey ?? Array.Empty<byte>();
    }

    public override Span<byte> Decrypt(Span<byte> data)
    {
        var key1 = new byte[0x10];
        var key2 = new byte[0x10];
        var key3 = new byte[0x10];

        data.Slice(4, 0x10).CopyTo(key1);
        data.Slice(0x74, 0x10).CopyTo(key2);
        data.Slice(0x84, 0x10).CopyTo(key3);

        var encryptedBlockSize = Math.Min(0x10 * ((data.Length - 0x94) >> 7), BlockSize);

        if (!InitVector.IsNullOrEmpty())
            for (var i = 0; i < InitVector.Length; i++)
                key2[i] ^= InitVector[i];

        if (!SBox.IsNullOrEmpty())
            for (var i = 0; i < 0x10; i++)
                key1[i] = SBox[(i % 4 * 0x100) | key1[i]];

        AES.Decrypt(key1, ExpansionKey);
        AES.Decrypt(key3, ExpansionKey);

        for (var i = 0; i < key1.Length; i++) key1[i] ^= key3[i];

        key1.CopyTo(data.Slice(0x84, 0x10));

        var seed1 = BinaryPrimitives.ReadUInt64LittleEndian(key2);
        var seed2 = BinaryPrimitives.ReadUInt64LittleEndian(key3);
        var seed = seed2 ^ seed1 ^ (seed1 + (uint)data.Length - 20);

        var encryptedBlock = data.Slice(0x94, encryptedBlockSize);
        var seedSpan = BitConverter.GetBytes(seed);
        for (var i = 0; i < encryptedBlockSize; i++)
            encryptedBlock[i] ^= (byte)(seedSpan[i % seedSpan.Length] ^ BlockKey[i % BlockKey.Length]);

        data = data[0x14..];

        if (!PostKey.IsNullOrEmpty())
            for (var i = 0; i < 0xC00; i++)
                data[i] ^= PostKey[i % PostKey.Length];

        return data;
    }

    public static bool IsMr0k(ReadOnlySpan<byte> data)
    {
        return data[..4].SequenceEqual(mr0kMagic);
    }
}