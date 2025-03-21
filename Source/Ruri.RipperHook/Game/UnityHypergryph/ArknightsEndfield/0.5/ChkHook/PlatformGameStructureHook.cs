using AssetRipper.Import.Logging;
using AssetRipper.Import.Structure.Platforms;
using AssetRipper.IO.Endian;
using System.Reflection;

namespace Ruri.RipperHook.ArknightsEndfield_0_5;

public partial class ArknightsEndfield_0_5_Hook
{
    public static bool CustomAssetBundlesCheck(FileInfo file)
    {
        return file.Extension == ".chk";
    }
}