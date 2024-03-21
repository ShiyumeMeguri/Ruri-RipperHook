using AssetRipper.Import.Structure.Platforms;
using AssetRipper.IO.Endian;
using System.Reflection;

namespace Ruri.RipperHook.StarRailCommon;

public partial class StarRailCommon_Hook
{
    public static bool CustomAssetBundlesCheckMagicNum(EndianReader reader, MethodInfo FromSerializedFile)
    {
        return (bool)FromSerializedFile.Invoke(null, new object[] { reader, "ENCR" });
    }
}