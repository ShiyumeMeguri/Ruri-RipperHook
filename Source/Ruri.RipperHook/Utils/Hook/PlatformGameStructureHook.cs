using AssetRipper.Import.Structure.Platforms;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook;

public class PlatformGameStructureHook : CommonHook
{
    private static readonly MethodInfo AddAssetBundle = typeof(PlatformGameStructure).GetMethod("AddAssetBundle", ReflectionExtensions.PrivateInstanceBindFlag());

    public delegate bool AssetBundlesCheckDelegate(FileInfo file);

    public static AssetBundlesCheckDelegate CustomAssetBundlesCheck;

    [RetargetMethod(typeof(PlatformGameStructure), nameof(CollectAssetBundles), 2)]
    private void CollectAssetBundles(DirectoryInfo root, IDictionary<string, string> files)
    {
        foreach (FileInfo file in root.EnumerateFiles())
        {
            if (CustomAssetBundlesCheck(file))
            {
                string name = Path.GetFileNameWithoutExtension(file.Name).ToLowerInvariant();
                AddAssetBundle.Invoke(this, new object[] { files, name, file.FullName });
            }
        }
    }
}