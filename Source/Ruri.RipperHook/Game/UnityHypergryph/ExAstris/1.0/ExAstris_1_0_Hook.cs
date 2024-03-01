using Ruri.RipperHook.Crypto;
using Ruri.RipperHook.ExAstrisCommon;
using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;

namespace Ruri.RipperHook.ExAstris_1_0;

public partial class ExAstris_1_0_Hook : RipperHook
{
    protected ExAstris_1_0_Hook()
    {
        ExAstrisCommon_Hook.CustomLZ4 = new LZ4Lit();
        BundleFileBlockReaderHook.CustomBlockCompression = ExAstrisCommon_Hook.CustomBlockCompression;
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(BundleFileBlockReaderHook).Namespace);
        additionalNamespaces.Add(typeof(ExAstrisCommon_Hook).Namespace);
        base.InitAttributeHook();
    }
}