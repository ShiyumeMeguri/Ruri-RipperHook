using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Streams.Smart;
using K4os.Compression.LZ4;

namespace Ruri.RipperHook.ArknightsEndfieldCommon;

public partial class ArknightsEndfieldCommon_Hook
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
            case (CompressionType)5:
                var compressedSize = block.CompressedSize;
                var uncompressedSize = block.UncompressedSize;
                var uncompressedBytes = new byte[uncompressedSize];
                Span<byte> compressedBytes = new BinaryReader(m_stream).ReadBytes((int)block.CompressedSize);

                if (m_cachedBlockIndex == 0)
                    compressedBytes = RuriRuntimeHook.commonDecryptor.Decrypt(compressedBytes);

                var bytesWritten = CustomLZ4.Decompress(compressedBytes, uncompressedBytes);
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