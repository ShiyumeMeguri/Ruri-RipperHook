using System;

namespace Ruri.RipperHook.Crypto;
public class LZ4_ExAstris : LZ4
{
    public new static LZ4_ExAstris Instance => new();
    protected override (int encCount, int litCount) GetLiteralToken(ReadOnlySpan<byte> cmp, ref int cmpPos) => ((cmp[cmpPos] >> 4) & 0xf, (cmp[cmpPos++] >> 0) & 0xf);
}