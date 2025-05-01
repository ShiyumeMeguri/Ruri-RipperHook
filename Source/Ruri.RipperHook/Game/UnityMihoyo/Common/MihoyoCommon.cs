using AssetRipper.Import.Logging;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Exceptions;
using AssetRipper.IO.Files.Streams.Smart;
using K4os.Compression.LZ4;
using Ruri.RipperHook.Crypto;
using System.Buffers;

namespace Ruri.RipperHook.UnityMihoyo;
public static class MihoyoCommon
{
    enum CustomCompressionType
    {
        OodleHSR = 6,
        OodleMr0k = 7,
        Oodle = 9,
    }

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
            case (CompressionType)CustomCompressionType.OodleHSR:
            case (CompressionType)CustomCompressionType.OodleMr0k:
                var uncompressedSize1 = block.UncompressedSize;
                var uncompressedBytes1 = new byte[uncompressedSize1];
                var compressedSize1 = block.CompressedSize;
                Span<byte> compressedBytes1 = new BinaryReader(m_stream).ReadBytes((int)block.CompressedSize);

                if (compressType == (CompressionType)CustomCompressionType.OodleMr0k && Mr0kDecryptor.IsMr0k(compressedBytes1))
                    compressedBytes1 = RuriRuntimeHook.commonDecryptor.Decrypt(compressedBytes1);
                //var bytesWritten1 = LZ4Codec.Decode(compressedBytes1, uncompressedBytes1);
                var bytesWritten1 = OodleHelper.Decompress(compressedBytes1, uncompressedBytes1);
                if (bytesWritten1 < 0)
                {
                    throw new Exception($"bytesWritten < 0");
                }
                else if (bytesWritten1 != uncompressedSize1)
                {
                    throw new Exception($"bytesWritten != uncompressedSize, {compressType}, {uncompressedSize1}, {bytesWritten1}");
                }
                new MemoryStream(uncompressedBytes1).CopyTo(m_cachedBlockStream);
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
                            findIndex = 0;
                            if (currentFile.Count > findSpan.Length)
                            {
                                var file = currentFile.GetRange(0, currentFile.Count - findSpan.Length).ToArray();
                                files.Add(file);
                                currentFile.RemoveRange(0, file.Length);
                            }
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