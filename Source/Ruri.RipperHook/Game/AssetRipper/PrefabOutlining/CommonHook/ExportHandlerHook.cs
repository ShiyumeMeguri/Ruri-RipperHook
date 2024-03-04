using AssetRipper.Processing;
using AssetRipper.Export.UnityProjects.Configuration;
using AssetRipper.Processing.PrefabOutlining;

namespace Ruri.RipperHook.AR_PrefabOutlining;

public partial class AR_PrefabOutlining_Hook
{
    public static IEnumerable<IAssetProcessor> PrefabOutliningProcessor(LibraryConfiguration Settings)
    {
        if (Settings.ProcessingSettings.EnablePrefabOutlining)
        {
            yield return new PrefabOutliningProcessor();
        }
    }
}