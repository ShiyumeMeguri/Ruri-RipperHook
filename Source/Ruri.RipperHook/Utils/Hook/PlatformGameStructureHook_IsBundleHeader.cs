using AssetRipper.Import.Structure.Platforms;
using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook_IsBundleHeader;

public class PlatformGameStructureHook_IsBundleHeader : CommonHook
{
    private static readonly MethodInfo FromSerializedFile = typeof(BundleHeader).GetMethod("IsBundleHeader", ReflectionExtensions.PrivateStaticBindFlag());

    // 自定义文件头魔数检测
    public delegate bool AssetBundlesMagicNumCheckDelegate(EndianReader reader, MethodInfo FromSerializedFile);

    public static AssetBundlesMagicNumCheckDelegate CustomAssetBundlesCheckMagicNum;

    [RetargetMethod(typeof(FileStreamBundleHeader), nameof(IsBundleHeader), 0)]
    public static bool IsBundleHeader(EndianReader reader) => CustomAssetBundlesCheckMagicNum(reader, FromSerializedFile);
}