using Ruri.RipperHook.HookUtils.BundleFileBlockReaderHook;
using Ruri.RipperHook.UnityChinaCommon;

namespace Ruri.RipperHook.PunishingGrayRaven_2_11;

public partial class PunishingGrayRaven_2_11_Hook : RipperHook
{
    protected PunishingGrayRaven_2_11_Hook()
    {
        UnityChinaCommon_Hook.SetKey("PGR CN/JP/TW", "7935585076714C4F72436F6B57524961");
        BundleFileBlockReaderHook.CustomBlockCompression = UnityChinaCommon_Hook.CustomBlockCompression;
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(UnityChinaCommon_Hook).Namespace);
        additionalNamespaces.Add(typeof(BundleFileBlockReaderHook).Namespace);
        base.InitAttributeHook();
    }
}