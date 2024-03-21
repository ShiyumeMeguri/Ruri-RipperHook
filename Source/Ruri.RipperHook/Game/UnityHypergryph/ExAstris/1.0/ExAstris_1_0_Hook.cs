using Ruri.RipperHook.Crypto;
using Ruri.RipperHook.ExAstrisCommon;
using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

namespace Ruri.RipperHook.ExAstris_1_0;

public partial class ExAstris_1_0_Hook : RipperHook
{
    protected ExAstris_1_0_Hook()
    {
        ExAstrisCommon_Hook.CustomLZ4 = new LZ4_ExAstris();
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(ExAstrisCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = ExAstrisCommon_Hook.CustomBlockCompression; });
        base.InitAttributeHook();
    }
}