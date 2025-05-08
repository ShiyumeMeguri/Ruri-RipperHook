using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.Text.Html;

namespace Ruri.RipperHook.ZenlessZoneZeroCommon;

// m_Header 读取
public partial class ZenlessCommon_Hook
{
    [RetargetMethod(typeof(FileStreamBundleHeader), nameof(Read))]
    public void Read(EndianReader reader)
    {
        var _this = (object)this as FileStreamBundleHeader;

        string signature = reader.ReadStringZeroTerm(); // mhy1
        _this.Version = BundleVersion.BF_520_x;
        _this.UnityWebBundleVersion = "5.x.x";
        _this.UnityWebMinimumRevision = "2019.4.40f12";

        //_this.Size = reader.ReadInt64(); // 没这玩意
        _this.CompressedBlocksInfoSize = reader.ReadInt32();
        //_this.UncompressedBlocksInfoSize = reader.ReadInt32();  // 加密了, 到 Blockinfo 那里读
        _this.Flags = (BundleFlags)0x45;
    }
}