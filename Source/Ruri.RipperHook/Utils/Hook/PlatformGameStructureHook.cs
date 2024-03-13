using AssetRipper.Assets.Collections;
using AssetRipper.Import.Logging;
using AssetRipper.Import.Structure.Platforms;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook;

public class PlatformGameStructureHook : CommonHook
{
    private static readonly MethodInfo CollectAssetBundlesRecursively = typeof(PlatformGameStructure).GetMethod("CollectAssetBundlesRecursively", ReflectionExtensions.PrivateInstanceBindFlag());
    private static readonly MethodInfo AddAssetBundle = typeof(PlatformGameStructure).GetMethod("AddAssetBundle", ReflectionExtensions.PrivateInstanceBindFlag());

    public delegate bool AssetBundlesCheckDelegate(FileInfo file);

    public static AssetBundlesCheckDelegate CustomAssetBundlesCheck;

    public delegate bool CollectStreamingAssetsDelegate(PlatformGameStructure _this, IDictionary<string, string> files, MethodInfo CollectAssetBundlesRecursively);

    public static CollectStreamingAssetsDelegate CustomCollectStreamingAssets;

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

    [RetargetMethod(typeof(PlatformGameStructure), nameof(CollectStreamingAssets))]
    protected void CollectStreamingAssets(IDictionary<string, string> files)
    {
        var _this = (object)this as PlatformGameStructure;
        if (CustomCollectStreamingAssets(_this, files, CollectAssetBundlesRecursively)) return;

        if (string.IsNullOrWhiteSpace(_this.StreamingAssetsPath))
        {
            return;
        }

        Logger.Info(LogCategory.Import, "Collecting Streaming Assets...");
        DirectoryInfo streamingDirectory = new DirectoryInfo(_this.StreamingAssetsPath);
        if (streamingDirectory.Exists)
        {
            CollectAssetBundlesRecursively.Invoke(this, new object[] { streamingDirectory, files });
        }
    }
}