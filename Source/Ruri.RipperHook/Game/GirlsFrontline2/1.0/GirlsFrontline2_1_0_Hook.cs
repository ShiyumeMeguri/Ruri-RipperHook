using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook.GirlsFrontline2_1_0;

public partial class GirlsFrontline2_1_0_Hook : AssetHook
{
    public static readonly byte[] XorKey = { 0x55, 0x6E, 0x69, 0x74, 0x79, 0x46, 0x53, 0x00, 0x00, 0x00, 0x00, 0x07, 0x35, 0x2E, 0x78, 0x2E };

    protected GirlsFrontline2_1_0_Hook()
    {
        RuriRuntimeHook.gameCrypto = new GF2Xor(XorKey);
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add("Ruri.RipperHook.GirlsFrontline2Common");
        base.InitAttributeHook();
    }
}