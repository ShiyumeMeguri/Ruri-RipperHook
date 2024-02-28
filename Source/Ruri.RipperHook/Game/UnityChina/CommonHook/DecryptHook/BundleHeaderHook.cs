using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook.UnityChinaCommon;

public partial class UnityChinaCommon_Hook
{
    [RetargetMethod(typeof(FileStreamBundleHeader), nameof(Read))]
    public void Read(EndianReader reader)
    {
        var _this = (object)this as FileStreamBundleHeader;

        string signature = reader.ReadStringZeroTerm();
        _this.Version = (BundleVersion)reader.ReadInt32();
        _this.UnityWebBundleVersion = reader.ReadStringZeroTerm();
        _this.UnityWebMinimumRevision = reader.ReadStringZeroTerm();

        _this.Size = reader.ReadInt64();
        _this.CompressedBlocksInfoSize = reader.ReadInt32();
        _this.UncompressedBlocksInfoSize = reader.ReadInt32();
        _this.Flags = (BundleFlags)reader.ReadInt32();
        if (!_this.Flags.GetBlocksInfoAtTheEnd())
        {
            RuriRuntimeHook.unityChinaDecryptor[_this] = new UnityChinaDecryptor(reader);
        }
    }
}