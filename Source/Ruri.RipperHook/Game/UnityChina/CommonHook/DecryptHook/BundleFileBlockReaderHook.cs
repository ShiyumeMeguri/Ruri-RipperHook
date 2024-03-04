using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Exceptions;
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
                uint uncompressedSize = block.UncompressedSize;
                byte[] uncompressedBytes = new byte[uncompressedSize];
                var compressedSize = block.CompressedSize;
                Span<byte> compressedBytes = new BinaryReader(m_stream).ReadBytes((int)block.CompressedSize);
                
                if ((block.Flags & (StorageBlockFlags)0x100) != 0)
                    RuriRuntimeHook.unityChinaDecryptor.DecryptBlock(compressedBytes, (int)compressedSize, m_cachedBlockIndex);
                
                var bytesWritten = LZ4Codec.Decode(compressedBytes, uncompressedBytes);
                if (bytesWritten < 0)
                {
                    throw new Exception($"bytesWritten < 0");
                }
                else if (bytesWritten != uncompressedSize)
                {
                    throw new Exception($"bytesWritten != uncompressedSize, {compressType}, {uncompressedSize}, {bytesWritten}");
                }
                new MemoryStream(uncompressedBytes).CopyTo(m_cachedBlockStream);
                break;

            case CompressionType.Lzham:
                UnsupportedBundleDecompression.ThrowLzham("CompressionType.Lzham");
                break;

            default:
                if (ZstdCompression.IsZstd(m_stream))
                {
                    ZstdCompression.DecompressStream(m_stream, block.CompressedSize, m_cachedBlockStream, block.UncompressedSize);
                }
                else
                {
                    UnsupportedBundleDecompression.Throw("UnsupportedBundleDecompression", compressType);
                }
                break;
        }
    }
}