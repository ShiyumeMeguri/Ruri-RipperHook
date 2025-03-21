using Ruri.RipperHook.ArknightsEndfieldCommon;
using Ruri.RipperHook.Crypto;
using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.HookUtils.PlatformGameStructureHook_CollectAssetBundles;

namespace Ruri.RipperHook.ArknightsEndfield_0_5;

public partial class ArknightsEndfield_0_5_Hook : RipperHook
{
    protected static LZ4_ArknightsEndfield_0_5 customLZ4;
    protected ArknightsEndfield_0_5_Hook()
    {
        customLZ4 = new LZ4_ArknightsEndfield_0_5();
        RuriRuntimeHook.commonDecryptor = new FairGuardDecryptor_ArknightsEndfield_0_5();
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ArknightsEndfieldCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = CustomBlockCompression; });
        AddExtraHook(typeof(PlatformGameStructureHook_CollectAssetBundles).Namespace, () => { PlatformGameStructureHook_CollectAssetBundles.CustomAssetBundlesCheck = CustomAssetBundlesCheck; });
        base.InitAttributeHook();
    }
}