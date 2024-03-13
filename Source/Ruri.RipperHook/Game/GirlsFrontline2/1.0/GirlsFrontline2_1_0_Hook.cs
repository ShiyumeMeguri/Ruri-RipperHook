using Ruri.RipperHook.GirlsFrontline2Common;
using Ruri.RipperHook.HookUtils.GameBundleHook;
using Ruri.RipperHook.HookUtils.PlatformGameStructureHook;

namespace Ruri.RipperHook.GirlsFrontline2_1_0;

public partial class GirlsFrontline2_1_0_Hook : RipperHook
{
    public static readonly byte[] XorKey = { 0x55, 0x6E, 0x69, 0x74, 0x79, 0x46, 0x53, 0x00, 0x00, 0x00, 0x00, 0x07, 0x35, 0x2E, 0x78, 0x2E };

    protected GirlsFrontline2_1_0_Hook()
    {
        RuriRuntimeHook.commonDecryptor = new XorDecryptor(XorKey);
        GameBundleHook.CustomFilePreInitialize = GirlsFrontline2Common_Hook.CustomFilePreInitialize;
        PlatformGameStructureHook.CustomAssetBundlesCheck = GirlsFrontline2Common_Hook.CustomAssetBundlesCheck;
        PlatformGameStructureHook.CustomCollectStreamingAssets = GirlsFrontline2Common_Hook.CustomCollectStreamingAssets;
    }

    protected override void InitAttributeHook()
    {
        additionalNamespaces.Add(typeof(GirlsFrontline2Common_Hook).Namespace);
        additionalNamespaces.Add(typeof(GameBundleHook).Namespace);
        additionalNamespaces.Add(typeof(PlatformGameStructureHook).Namespace);
        base.InitAttributeHook();
    }
}