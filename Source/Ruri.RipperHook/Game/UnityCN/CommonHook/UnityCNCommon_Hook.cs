using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook.UnityCNCommon;

public partial class UnityCNCommon_Hook : AssetHook
{
    public static void SetKey(string name, string key)
    {
        UnityCN.SetKey(new UnityCN.Entry(name, key));
    }
}