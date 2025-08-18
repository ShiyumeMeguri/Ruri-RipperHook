using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.Primitives;
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
            RuriRuntimeHook.unityChinaDecryptor = new UnityChinaDecryptor(reader);
            var version = UnityVersion.Parse(_this.UnityWebMinimumRevision);
            if (version.Major < 2020 ||                                                 // 2019 and earlier
               (version.Major == 2020 && version.Minor == 3 && version.Build <= 34) ||  // 2020.3.34 and earlier
               (version.Major == 2021 && version.Minor == 3 && version.Build <= 2) ||   // 2021.3.2 and earlier
               (version.Major == 2022 && version.Minor == 3 && version.Build <= 1))     // 2022.3.1 and earlier
            {
                _this.Flags &= ~BundleFlags.EncryptionOld;
            }
        }
    }
}