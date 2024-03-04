using AssetRipper.GUI.Web;

namespace Ruri.RipperHook;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Hook(args);
        RunAssetRipper();
    }

    private static void Hook(string[] args)
    {
        RuriRuntimeHook.Init(GameHookType.AR_StaticMeshSeparation);
        RuriRuntimeHook.Init(GameHookType.AR_PrefabOutlining);
    }

    private static void RunAssetRipper()
    {
        WebApplicationLauncher.Launch();
    }
}