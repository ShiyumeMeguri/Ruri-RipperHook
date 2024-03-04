using AssetRipper.Processing;
using AssetRipper.Export.UnityProjects.Configuration;

namespace Ruri.RipperHook.AR_StaticMeshSeparation;

public partial class AR_StaticMeshSeparation_Hook
{
    public static IEnumerable<IAssetProcessor> StaticMeshProcessor(LibraryConfiguration Settings)
    {
        if (Settings.ProcessingSettings.EnableStaticMeshSeparation)
        {
            yield return new StaticMeshProcessor();
        }
    }
}