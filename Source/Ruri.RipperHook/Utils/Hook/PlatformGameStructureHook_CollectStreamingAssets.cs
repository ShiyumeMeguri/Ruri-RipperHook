using AssetRipper.Import.Logging;
using AssetRipper.Import.Structure.Platforms;
using System.Reflection;

namespace Ruri.RipperHook.HookUtils.PlatformGameStructureHook_CollectStreamingAssets;

public class PlatformGameStructureHook_CollectStreamingAssets : CommonHook
{
    private static readonly MethodInfo CollectAssetBundlesRecursively = typeof(PlatformGameStructure).GetMethod("CollectAssetBundlesRecursively", ReflectionExtensions.PrivateInstanceBindFlag());

    // 自定义流文件夹检测
    public delegate bool CollectStreamingAssetsDelegate(PlatformGameStructure _this, IDictionary<string, string> files, MethodInfo CollectAssetBundlesRecursively);

    public static CollectStreamingAssetsDelegate CustomCollectStreamingAssets;

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