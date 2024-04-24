using AssetRipper.IO.Endian;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.BundleFiles.FileStream;

namespace Ruri.RipperHook.StarRailCommon;

public partial class StarRailCommon_Hook
{
    [RetargetMethod(typeof(BlocksInfo), nameof(Read))]
    public static BlocksInfo BlocksInfo_Read(EndianReader reader)
    {
        return new BlocksInfo(new Hash128(), reader.ReadEndianArray<StorageBlock>());
    }
}