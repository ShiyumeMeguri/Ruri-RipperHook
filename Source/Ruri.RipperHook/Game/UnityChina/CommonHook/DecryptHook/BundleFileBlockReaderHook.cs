using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Streams.Smart;
using K4os.Compression.LZ4;

namespace Ruri.RipperHook.UnityChinaCommon;

public partial class UnityChinaCommon_Hook
{
    public static void CustomBlockCompression(Stream m_stream, StorageBlock block, SmartStream m_cachedBlockStream, CompressionType compressType, int m_cachedBlockIndex)
    {
        switch (compressType)
        {
            case CompressionType.Lzma:
                LzmaCompression.DecompressLzmaStream(m_stream, block.CompressedSize, m_cachedBlockStream, block.UncompressedSize);
                break;

            case CompressionType.Lz4:
            case CompressionType.Lz4HC:
                var compressedSize = block.CompressedSize;
                var uncompressedSize = block.UncompressedSize;
                var uncompressedBytes = new byte[uncompressedSize];
                Span<byte> compressedBytes = new BinaryReader(m_stream).ReadBytes((int)block.CompressedSize);
                // 解密处
                if ((block.Flags & (StorageBlockFlags)0x100) != 0)
                {
                    RuriRuntimeHook.unityChinaDecryptor.DecryptBlock(compressedBytes, (int)compressedSize, m_cachedBlockIndex);
                }

                var bytesWritten = LZ4Codec.Decode(compressedBytes, uncompressedBytes);
                if (bytesWritten < 0)
                    throw new Exception("EncryptedFileException.Throw(entry.PathFixed)");
                else if (bytesWritten != uncompressedSize)
                    throw new Exception("DecompressionFailedException.ThrowIncorrectNumberBytesWritten(entry.PathFixed, uncompressedSize, bytesWritten)");
                new MemoryStream(uncompressedBytes).CopyTo(m_cachedBlockStream);
                break;

            default:
                throw new NotSupportedException($"Bundle compression '{compressType}' isn't supported");
        }
    }
}