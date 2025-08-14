using System.Reflection;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.Exceptions;
using AssetRipper.IO.Files.Extensions;
using K4os.Compression.LZ4;
using Ruri.RipperHook.UnityMihoyo;

namespace Ruri.RipperHook.HoukaiCommon;

public partial class HoukaiCommon_Hook
{
    private static readonly MethodInfo ReadMetadata = typeof(FileStreamBundleFile).GetMethod("ReadMetadata", ReflectionExtensions.PrivateInstanceBindFlag());

    [RetargetMethod(typeof(FileStreamBundleFile), nameof(ReadFileStreamMetadata))]
    public void ReadFileStreamMetadata(Stream stream, long basePosition)
    {
        var _this = (object)this as FileStreamBundleFile;

        var Header = _this.Header;
        var NameFixed = _this.NameFixed;
        if (Header.Version >= BundleVersion.BF_LargeFilesSupport) stream.Align(16);
        if (Header.Flags.GetBlocksInfoAtTheEnd())
        {
            stream.Position = basePosition + (Header.Size - Header.CompressedBlocksInfoSize);
        }

        var metaCompression = Header.Flags.GetCompression();
        var compressedBytes = new BinaryReader(stream).ReadBytes(Header.CompressedBlocksInfoSize);
        switch (metaCompression)
        {
            case CompressionType.None:
            {
                ReadMetadata.Invoke(this, new object[] { stream, Header.UncompressedBlocksInfoSize });
            }
                break;

            case CompressionType.Lzma:
            {
                using var uncompressedStream = new MemoryStream(new byte[Header.UncompressedBlocksInfoSize]);
                LzmaCompression.DecompressLzmaStream(stream, Header.CompressedBlocksInfoSize, uncompressedStream, Header.UncompressedBlocksInfoSize);

                uncompressedStream.Position = 0;
                ReadMetadata.Invoke(this, new object[] { uncompressedStream, Header.UncompressedBlocksInfoSize });
            }
                break;

            case CompressionType.Lz4:
            case CompressionType.Lz4HC:
                {
                    var uncompressedSize = Header.UncompressedBlocksInfoSize;
                    var uncompressedBytes = new byte[uncompressedSize];
                    var bytesWritten = LZ4Codec.Decode(compressedBytes, uncompressedBytes);
                    if (bytesWritten < 0)
                    {
                        ARIntelnalReflection.ThrowNoBytesWrittenMethod.Invoke(null, new object[] { _this.NameFixed, metaCompression });
                    }
                    else if (bytesWritten != uncompressedSize)
                    {
                        ARIntelnalReflection.ThrowIncorrectNumberBytesWrittenMethod.Invoke(null, new object[] { _this.NameFixed, metaCompression, (long)uncompressedSize, (long)bytesWritten });
                    }
                    ReadMetadata.Invoke(this, new object[] { new MemoryStream(uncompressedBytes), uncompressedSize });
                }
                break;

            case (CompressionType)5:
                if (Mr0kDecryptor.IsMr0k(compressedBytes))
                    compressedBytes = RuriRuntimeHook.commonDecryptor.Decrypt(compressedBytes).ToArray();
                goto case CompressionType.Lz4HC;

            default:
                UnsupportedBundleDecompression.Throw(NameFixed, metaCompression);
                break;
        }
    }
}