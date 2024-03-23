using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Exceptions;
using AssetRipper.IO.Files.Streams.Smart;
using K4os.Compression.LZ4;

namespace Ruri.RipperHook.UnityMihoyo;
public static class MihoyoCommon
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
                uint uncompressedSize = block.UncompressedSize;
                byte[] uncompressedBytes = new byte[uncompressedSize];
                var compressedSize = block.CompressedSize;
                Span<byte> compressedBytes = new BinaryReader(m_stream).ReadBytes((int)block.CompressedSize);

                if (compressType == (CompressionType)5 && Mr0kDecryptor.IsMr0k(compressedBytes))
                    compressedBytes = RuriRuntimeHook.commonDecryptor.Decrypt(compressedBytes);

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
    public static List<BlockAssetInfo> FindBlockFiles(SmartStream stream, byte[] findBytes, string path)
    {
        List<BlockAssetInfo> assets = new List<BlockAssetInfo>();
        long fileSize = stream.Length;
        byte[] buffer = new byte[findBytes.Length];

        long currentOffset = 0;
        while (currentOffset < fileSize)
        {
            stream.Position = currentOffset;
            stream.Read(buffer, 0, buffer.Length);
            if (buffer.StartsWith(findBytes))
            {
                long fileBlockStart = currentOffset;
                long nextEncrStart = fileBlockStart + buffer.Length;
                bool foundNextHead = false;
                while (nextEncrStart < fileSize)
                {
                    stream.Position = nextEncrStart;
                    stream.Read(buffer, 0, buffer.Length);
                    if (buffer.StartsWith(findBytes))
                    {
                        foundNextHead = true;
                        break;
                    }
                    nextEncrStart++;
                }

                if (!foundNextHead)
                    nextEncrStart += buffer.Length;

                long fileBlockSize = nextEncrStart - fileBlockStart;
                assets.Add(new BlockAssetInfo { FilePath = path + fileBlockStart, FileSize = (int)fileBlockSize, Offset = (int)fileBlockStart });
                currentOffset = nextEncrStart;
            }
            else
            {
                currentOffset++;
            }
        }
        return assets;
    }
}