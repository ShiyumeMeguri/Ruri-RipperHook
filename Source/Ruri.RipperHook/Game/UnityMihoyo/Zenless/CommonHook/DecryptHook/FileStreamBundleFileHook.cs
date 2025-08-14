using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using K4os.Compression.LZ4;
using Ruri.RipperHook.UnityMihoyo;
using System.Reflection;

// ReadBlocksInfoAndDirectory
namespace Ruri.RipperHook.ZenlessZoneZeroCommon;
public partial class ZenlessCommon_Hook
{
    private static readonly MethodInfo ReadMetadata = typeof(FileStreamBundleFile).GetMethod("ReadMetadata", ReflectionExtensions.PrivateInstanceBindFlag());

    [RetargetMethod(typeof(FileStreamBundleFile), nameof(ReadFileStreamMetadata))]
    public void ReadFileStreamMetadata(Stream stream, long basePosition)
    {
        var _this = (object)this as FileStreamBundleFile;
        var Header = _this.Header;

        var metaCompression = Header.Flags.GetCompression();

        var blocksInfo = new BinaryReader(stream).ReadBytes(Header.CompressedBlocksInfoSize);

        Mhy1Decryptor.DescrambleHeader(blocksInfo);

        var blocksInfoReader = new BinaryReader(new MemoryStream(blocksInfo, 48, (int)Header.CompressedBlocksInfoSize - 48));

        Header.UncompressedBlocksInfoSize = (int)MihoyoReader.ReadMhyUInt(blocksInfoReader);

        var compressedBytes = blocksInfoReader.ReadBytes((int)(blocksInfoReader.BaseStream.Length - blocksInfoReader.BaseStream.Position));

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
        if (bytesWritten < 0)
            throw new Exception("EncryptedFileException.Throw(NameFixed)");
        else if (bytesWritten != uncompressedSize)
            throw new Exception("DecompressionFailedException.ThrowIncorrectNumberBytesWritten(NameFixed, uncompressedSize, bytesWritten)");
        ReadMetadata.Invoke(this, new object[] { new MemoryStream(uncompressedBytes), uncompressedSize });
    }
}

