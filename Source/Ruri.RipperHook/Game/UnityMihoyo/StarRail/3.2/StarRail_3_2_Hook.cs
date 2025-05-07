using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.HookUtils.GameBundleHook;
using Ruri.RipperHook.HookUtils.PlatformGameStructureHook_IsBundleHeader;
using Ruri.RipperHook.StarRailCommon;
using Ruri.RipperHook.UnityMihoyo;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook : RipperHook
{
    public const string ClassHookVersion = "2019.4.34f1";
    protected StarRail_3_2_Hook()
    {
        RuriRuntimeHook.commonDecryptor = new Mr0kDecryptor(Mr0kKey.Mr0kExpansionKey, initVector: Mr0kKey.Mr0kInitVector, blockKey: Mr0kKey.Mr0kBlockKey);
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(StarRailCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = MihoyoCommon.CustomBlockCompression; });
        AddExtraHook(typeof(PlatformGameStructureHook_IsBundleHeader).Namespace, () => { PlatformGameStructureHook_IsBundleHeader.CustomAssetBundlesCheckMagicNum = StarRailCommon_Hook.CustomAssetBundlesCheckMagicNum; });
        AddExtraHook(typeof(GameBundleHook).Namespace, () => { GameBundleHook.CustomFilePreInitialize = StarRailCommon_Hook.CustomFilePreInitialize; });
        base.InitAttributeHook();
    }
}