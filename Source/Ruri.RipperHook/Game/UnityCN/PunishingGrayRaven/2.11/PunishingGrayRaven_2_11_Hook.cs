using Ruri.RipperHook.UnityCNCommon;

namespace Ruri.RipperHook.PunishingGrayRaven_2_11;

public partial class PunishingGrayRaven_2_11_Hook : AssetHook
{
    protected PunishingGrayRaven_2_11_Hook()
    {
        RuriRuntimeHook.gameCrypto = new();
        UnityCNCommon_Hook.SetKey("PGR CN/JP/TW", "7935585076714C4F72436F6B57524961");
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add("Ruri.RipperHook.UnityCNCommon");
        base.InitAttributeHook();
    }
}