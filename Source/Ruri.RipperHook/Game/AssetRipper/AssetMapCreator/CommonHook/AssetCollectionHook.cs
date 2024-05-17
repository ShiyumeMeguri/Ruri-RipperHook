using AssetRipper.Assets;
using AssetRipper.Assets.Collections;
using AssetRipper.GUI.Web;
using AssetRipper.SourceGenerated;

namespace Ruri.RipperHook.AR_AssetMapCreator;

public partial class AR_AssetMapCreator_Hook
{
    [RetargetMethod(typeof(AssetCollection), nameof(AddAsset), isReturn: false)]
    private void AddAsset(IUnityObjectBase asset)
    {
        var bundleName = asset.AssetInfo.Collection.Bundle.Name;
        if (!assetClassIDLookup.ContainsKey(bundleName))
        {
            assetClassIDLookup[bundleName] = new HashSet<ClassIDType>();  // 如果没有，创建一个新的 HashSet
        }
        assetClassIDLookup[bundleName].Add((ClassIDType)asset.ClassID);
        ResolveDependencies(asset.Collection);
        string assetName = asset.ToString() == null ? "" : asset.ToString();
        if (!assetListLookup.ContainsKey(bundleName))
        {
            assetListLookup[bundleName] = new HashSet<string>();
        }
        assetListLookup[bundleName].Add(assetName);
    }

    private void ResolveDependencies(AssetCollection assetCollection)
    {
        if (!assetDependenciesLookup.ContainsKey(assetCollection.Bundle.Name))
        {
            assetDependenciesLookup[assetCollection.Bundle.Name] = new HashSet<string>();
        }

        if (assetCollection.Dependencies == null || assetCollection.Dependencies.Count == 0) return;

        foreach (var dependAsset in assetCollection.Dependencies)
        {
            if (dependAsset != null && assetDependenciesLookup[assetCollection.Bundle.Name].Add(dependAsset.Bundle.Name))
            {
                ResolveDependencies(dependAsset);
            }
        }
    }
}