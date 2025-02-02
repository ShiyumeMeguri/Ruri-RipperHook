using AssetRipper.Import.Logging;
using AssetRipper.Import.Structure.Platforms;
using AssetRipper.IO.Endian;
using System.Reflection;

namespace Ruri.RipperHook.GirlsFrontline2Common;

public partial class GirlsFrontline2Common_Hook
{
    public static bool CustomAssetBundlesCheck(FileInfo file)
    {
        return file.Extension == ".bundle";
    }
    public static bool CustomCollectStreamingAssets(PlatformGameStructure _this, List<KeyValuePair<string, string>> files, MethodInfo CollectAssetBundlesRecursively)
    {
        string localCachePath = Path.Combine(Path.GetDirectoryName(_this.StreamingAssetsPath), "LocalCache");
        if (string.IsNullOrWhiteSpace(_this.StreamingAssetsPath) && string.IsNullOrWhiteSpace(localCachePath)) return false;

        Logger.Info(LogCategory.Import, "Collecting Streaming Assets And LocalCache Assets...");
        DirectoryInfo streamingDirectory = new DirectoryInfo(_this.StreamingAssetsPath);
        if (streamingDirectory.Exists)
        {
            CollectAssetBundlesRecursively.Invoke(_this, new object[] { streamingDirectory, files });
        }
        DirectoryInfo localCache = new DirectoryInfo(localCachePath);
        if (localCache.Exists)
        {
            CollectAssetBundlesRecursively.Invoke(_this, new object[] { localCache, files });
        }
        return true;
    }
}