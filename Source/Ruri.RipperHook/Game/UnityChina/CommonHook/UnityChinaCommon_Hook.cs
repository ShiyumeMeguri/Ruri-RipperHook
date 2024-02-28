using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook.UnityChinaCommon;

public partial class UnityChinaCommon_Hook : RipperHook
{
    public static void SetKey(string name, string key)
    {
        UnityChinaDecryptor.SetKey(new UnityChinaDecryptor.Entry(name, key));
    }
}