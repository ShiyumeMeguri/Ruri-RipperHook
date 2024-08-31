using AssetRipper.GUI.Web;
using AssetRipper.SourceGenerated.Classes.ClassID_25;

namespace Ruri.RipperHook;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Hook(args);
        //Debug();
        RunAssetRipper(args);
    }

    private static void Hook(string[] args)
    {
        RuriRuntimeHook.Init(GameHookType.AR_StaticMeshSeparation);
        RuriRuntimeHook.Init(GameHookType.AR_PrefabOutlining);
        RuriRuntimeHook.Init(GameHookType.AR_BundledAssetsExportMode);
        RuriRuntimeHook.Init(GameHookType.AR_ExportDirectly);
        RuriRuntimeHook.Init(GameHookType.AR_AssetMapCreator);
        //RuriRuntimeHook.Init(GameHookType.AR_ShaderDecompiler);
        //RuriRuntimeHook.Init(GameHookType.AR_USCShaderDecompiler);
        //RuriRuntimeHook.Init(GameHookType.GirlsFrontline2_1_0);
        //RuriRuntimeHook.Init(GameHookType.Zenless_1_0);
    }

    private static void RunAssetRipper(string[] args)
    {
        WebApplicationLauncher.Launch(args);
    }
    private static void Debug()
    {
        DebugExtension.SubClassFinder(typeof(Renderer_2019_3_0_a6), "AssetRipper.SourceGenerated", "AssetRipper.SourceGenerated");
    }
}