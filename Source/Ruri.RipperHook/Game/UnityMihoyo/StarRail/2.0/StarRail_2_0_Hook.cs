using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.HookUtils.PlatformGameStructureHook_IsBundleHeader;
using Ruri.RipperHook.StarRailCommon;
using Ruri.RipperHook.UnityMihoyo;

namespace Ruri.RipperHook.StarRail_2_0;

public partial class StarRail_2_0_Hook : RipperHook
{
    protected StarRail_2_0_Hook()
    {
        RuriRuntimeHook.commonDecryptor = new Mr0kDecryptor(Mr0kKey.Mr0kExpansionKey, initVector: Mr0kKey.Mr0kInitVector, blockKey: Mr0kKey.Mr0kBlockKey);
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(StarRailCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = MihoyoCommon.CustomBlockCompression; });
        AddExtraHook(typeof(PlatformGameStructureHook_IsBundleHeader).Namespace, () => { PlatformGameStructureHook_IsBundleHeader.CustomAssetBundlesCheckMagicNum = StarRailCommon_Hook.CustomAssetBundlesCheckMagicNum; });
        base.InitAttributeHook();
    }
}