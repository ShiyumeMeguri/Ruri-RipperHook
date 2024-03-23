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
        RunAssetRipper();
    }

    private static void Hook(string[] args)
    {
        //RuriRuntimeHook.Init(GameHookType.AR_ShaderDecompiler);
        //RuriRuntimeHook.Init(GameHookType.AR_StaticMeshSeparation);
        RuriRuntimeHook.Init(GameHookType.AR_PrefabOutlining);
        RuriRuntimeHook.Init(GameHookType.StarRail_2_0);
    }

    private static void RunAssetRipper()
    {
        WebApplicationLauncher.Launch();
    }
    private static void Debug()
    {
        DebugExtension.SubClassFinder(typeof(Renderer_2019_3_0_a6), "AssetRipper.SourceGenerated", "AssetRipper.SourceGenerated");
    }
}