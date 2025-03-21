using Ruri.RipperHook.ArknightsEndfieldCommon;
using Ruri.RipperHook.Crypto;
using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

namespace Ruri.RipperHook.ArknightsEndfield_0_1;

public partial class ArknightsEndfield_0_1_Hook : RipperHook
{
    public static LZ4_ArknightsEndfield_0_1 customLZ4;
    protected ArknightsEndfield_0_1_Hook()
    {
        customLZ4 = new LZ4_ArknightsEndfield_0_1();
        RuriRuntimeHook.commonDecryptor = new FairGuardDecryptor();
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ArknightsEndfieldCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = ArknightsEndfield_0_1_Hook.CustomBlockCompression; });
        base.InitAttributeHook();
    }
}