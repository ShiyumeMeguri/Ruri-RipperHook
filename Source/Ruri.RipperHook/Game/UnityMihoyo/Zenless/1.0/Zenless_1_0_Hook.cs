using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.HookUtils.GameBundleHook;
using Ruri.RipperHook.HoukaiCommon;
using Ruri.RipperHook.UnityMihoyo;
using Ruri.RipperHook.ZenlessCommon;

namespace Ruri.RipperHook.Zenless_1_0;

public partial class Zenless_1_0_Hook : RipperHook
{

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ZenlessCommon_Hook).Namespace);
        //AddExtraHook(typeof(GameBundleHook).Namespace, () => { GameBundleHook.CustomFilePreInitialize = ZenlessCommon_Hook.CustomFilePreInitialize; });
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = MihoyoCommon.CustomBlockCompression; });
        base.InitAttributeHook();
    }
}