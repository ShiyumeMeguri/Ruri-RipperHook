using AssetRipper.Import.Structure.Platforms;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook_CollectAssetBundles;

public class PlatformGameStructureHook_CollectAssetBundles : CommonHook
{
    private static readonly MethodInfo AddAssetBundle = typeof(PlatformGameStructure).GetMethod("AddAssetBundle", ReflectionExtensions.AnyBindFlag());

    public delegate bool AssetBundlesCheckDelegate(FileInfo file);

    /// <summary>
    /// 自定义AB文件检测 针对少前2的XOR这种无法识别包的情况用
    /// 可以改为用后缀识别
    /// </summary>
    public static AssetBundlesCheckDelegate CustomAssetBundlesCheck;

    [RetargetMethod(typeof(PlatformGameStructure), nameof(CollectAssetBundles))]
    private static void CollectAssetBundles(DirectoryInfo root, List<KeyValuePair<string, string>> files)
    {
        foreach (FileInfo file in root.EnumerateFiles())
        {
            if (CustomAssetBundlesCheck(file))
            {
                string name = Path.GetFileNameWithoutExtension(file.Name).ToLowerInvariant();
                AddAssetBundle.Invoke(null, new object[] { files, name, file.FullName });
            }
        }
    }
}