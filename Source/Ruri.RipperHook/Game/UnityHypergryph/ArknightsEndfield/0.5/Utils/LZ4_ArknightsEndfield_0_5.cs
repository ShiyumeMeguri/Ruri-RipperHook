namespace Ruri.RipperHook.Crypto;

public class LZ4_ArknightsEndfield_0_5 : LZ4
{
    public new static LZ4_ArknightsEndfield_0_5 Instance => new();

    public override int Decompress(ReadOnlySpan<byte> cmp, Span<byte> dec)
    {
        int cmpPos = 0;
        int decPos = 0;

        // ReSharper disable once VariableHidesOuterVariable
        int GetLength(int length, ReadOnlySpan<byte> cmp)
        {
            byte sum;

            if (length == 0xf)
            {
                do
                {
                    length += sum = cmp[cmpPos++];
                } while (sum == 0xff);
            }

            return length;
        }

        do
        {
            byte token = cmp[cmpPos++];

            int encCount = (token >> 4) & 0xf;
            int litCount = (token >> 0) & 0xf;

            //Copy literal chunk
            litCount = GetLength(litCount, cmp);

            cmp.Slice(cmpPos, litCount).CopyTo(dec.Slice(decPos));

            cmpPos += litCount;
            decPos += litCount;

            if (cmpPos >= cmp.Length)
            {
                break;
            }

            //Copy compressed chunk
            int back = cmp[cmpPos++] << 8 | cmp[cmpPos++] << 0;

            encCount = GetLength(encCount, cmp) + 4;

            int encPos = decPos - back;

            if (encCount <= back)
            {
                dec.Slice(encPos, encCount).CopyTo(dec.Slice(decPos));

                decPos += encCount;
            }
            else
            {
                while (encCount-- > 0)
                {
                    dec[decPos++] = dec[encPos++];
                }
            }
        } while (cmpPos < cmp.Length && decPos < dec.Length);

        return decPos;
    }

    protected override (int encCount, int litCount) GetLiteralToken(ReadOnlySpan<byte> cmp, ref int cmpPos)
    {
        var val = cmp[cmpPos++];
        var lit = val & 0b00110011;
        var enc = val & 0b11001100;
        enc >>= 2;

        return ((enc & 0b11) | enc >> 2, (lit & 0b11) | lit >> 2);
    }
    protected override int GetChunkEnd(ReadOnlySpan<byte> cmp, ref int cmpPos) => cmp[cmpPos++] << 8 | cmp[cmpPos++] << 0;
}