using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.Text.Html;

namespace Ruri.RipperHook.StarRailCommon;

public partial class StarRailCommon_Hook
{
    [RetargetMethod(typeof(FileStreamBundleHeader), nameof(Read))]
    public void Read(EndianReader reader)
    {
        var _this = (object)this as FileStreamBundleHeader;

        string signature = reader.ReadStringZeroTerm();
        _this.Version = BundleVersion.BF_520_x; // is 7 but does not have uncompressedDataHash
        _this.UnityWebBundleVersion = "5.x.x";
        _this.UnityWebMinimumRevision = "2019.4.32f1";

        _this.Size = reader.ReadInt64();
        _this.CompressedBlocksInfoSize = reader.ReadInt32();
        _this.UncompressedBlocksInfoSize = reader.ReadInt32();
        _this.Flags = (BundleFlags)reader.ReadInt32();
    }
}