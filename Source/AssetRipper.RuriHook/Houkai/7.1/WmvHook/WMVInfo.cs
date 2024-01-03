using AssetRipper.IO.Files.CompressedFiles;
using AssetRipper.IO.Files.ResourceFiles;
using AssetRipper.IO.Files.SerializedFiles;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.Smart;
using System.Text;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.Assets.Bundles;
using AssetRipper.IO.Files.SerializedFiles.Parser;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.IO;
using AssetRipper.Primitives;
using System.Reflection;

namespace AssetRipper.RuriHook.Houkai_7_1;
public struct WMVInfo
{
    public struct UnitAssetInfo
    {
        public string FilePath { get; set; }
        public int Offset { get; set; }
        public int FileSize { get; set; }
    }
    public string FilePath { get; set; }
    public int FileSize { get; set; }
    public int FileCount { get; set; }
    public UnitAssetInfo[] UnitAssetArray { get; set; }
}
