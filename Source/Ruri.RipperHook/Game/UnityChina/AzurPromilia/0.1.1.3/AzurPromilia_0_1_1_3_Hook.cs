using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.UnityChinaCommon;

namespace Ruri.RipperHook.AzurPromilia_0_1_1_3;

public partial class AzurPromilia_0_1_1_3_Hook : RipperHook
{
    protected AzurPromilia_0_1_1_3_Hook()
    {
        UnityChinaCommon_Hook.SetKey("AzurPromilia_0_1_1_3", "7a346c32336268352333356826333231");
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(UnityChinaCommon_Hook).Namespace);
        AddExtraHook(typeof(BundleFileBlockReaderHook).Namespace, () => { BundleFileBlockReaderHook.CustomBlockCompression = UnityChinaCommon_Hook.CustomBlockCompression; });
        base.InitAttributeHook();
    }
}