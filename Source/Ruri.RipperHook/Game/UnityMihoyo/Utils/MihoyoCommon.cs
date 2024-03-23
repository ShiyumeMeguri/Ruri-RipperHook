using AssetRipper.Import.Logging;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Exceptions;
using AssetRipper.IO.Files.Streams.Smart;
using K4os.Compression.LZ4;
using System.Buffers;

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
    public static List<byte[]> FindBlockFiles(SmartStream stream, Span<byte> findSpan)
    {
        List<byte[]> files = new List<byte[]>();
        byte[] streamBuffer = ArrayPool<byte>.Shared.Rent(8192);
        List<byte> currentFile = new List<byte>();
        int findIndex = 0;

        try
        {
            int bytesRead;
            while ((bytesRead = stream.Read(streamBuffer, 0, streamBuffer.Length)) > 0)
            {
                Span<byte> bufferSpan = streamBuffer.AsSpan(0, bytesRead);
                for (int i = 0; i < bufferSpan.Length; i++)
                {
                    currentFile.Add(bufferSpan[i]);

                    if (bufferSpan[i] == findSpan[findIndex])
                    {
                        findIndex++;
                        if (findIndex == findSpan.Length)
                        {
                            // Found the pattern
                            if (currentFile.Count > findSpan.Length)
                            {
                                if (i - findSpan.Length >= 0)
                                {
                                   i -= findSpan.Length;
                                    files.Add(currentFile.GetRange(0, currentFile.Count - findSpan.Length).ToArray());
                                    currentFile.Clear();
                                }
                                else
                                {
                                    Logger.Warning($"遇到错误头 {files.Count}");
                                }
                            }
                            findIndex = 0;
                        }
                    }
                    else
                    {
                        findIndex = 0;
                    }
                }
            }

            if (currentFile.Count > 0)
            {
                files.Add(currentFile.ToArray());
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(streamBuffer);
        }
        return files;
    }
}