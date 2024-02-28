using System.Buffers.Binary;

namespace Ruri.RipperHook.Crypto;

public record CommonDecryptor
{
    public virtual Span<byte> Decrypt(Span<byte> dataSpan) => dataSpan;
}
