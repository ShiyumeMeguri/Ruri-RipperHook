using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.IO.Files.BundleFiles.FileStream;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook_IsBundleHeader;

public class PlatformGameStructureHook_IsBundleHeader : CommonHook
{
    private static readonly MethodInfo FromSerializedFile = typeof(BundleHeader).GetMethod("IsBundleHeader", ReflectionExtensions.PrivateStaticBindFlag());

    public delegate bool AssetBundlesMagicNumCheckDelegate(EndianReader reader, MethodInfo FromSerializedFile);

    /// <summary>
    /// 自定义文件头魔数检测 如果AB包的头不是以UnityFS开头的加密就可以用这个回调
    /// </summary>
    public static AssetBundlesMagicNumCheckDelegate CustomAssetBundlesCheckMagicNum;

    [RetargetMethod(typeof(FileStreamBundleHeader), nameof(IsBundleHeader))]
    public static bool IsBundleHeader(EndianReader reader) => CustomAssetBundlesCheckMagicNum(reader, FromSerializedFile);
}