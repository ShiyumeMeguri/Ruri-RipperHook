using AssetRipper.GUI.Web;
using AssetRipper.SourceGenerated.Classes.ClassID_25;
using System.Reflection;

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
        //RuriRuntimeHook.Init(GameHookType.AR_StaticMeshSeparation);
        //RuriRuntimeHook.Init(GameHookType.AR_PrefabOutlining);
        RuriRuntimeHook.Init(GameHookType.AR_BundledAssetsExportMode);
        RuriRuntimeHook.Init(GameHookType.AR_ExportDirectly);
        //RuriRuntimeHook.Init(GameHookType.AR_SkipProcessingAnimation);
        RuriRuntimeHook.Init(GameHookType.AR_AssetMapCreator);
        //RuriRuntimeHook.Init(GameHookType.AR_ShaderDecompiler);
        //RuriRuntimeHook.Init(GameHookType.AR_USCShaderDecompiler);
        //RuriRuntimeHook.Init(GameHookType.StarRail_3_2);
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
    private static void ExportAllGeneratedData()
    {
        Debug(); // 先强制加载dll
        ExportGeneratedData("AssetRipper.SourceGenerated.ReferenceAssembliesJsonData", @"D:\Downloads\assemblies.json");
        ExportGeneratedData("AssetRipper.SourceGenerated.EngineAssetsTpkData", @"D:\Downloads\engine_assets.tpk");
        // 这个被后处理过了 相当于Editor和Runtime时的区别 不能用
        //ExportGeneratedData("AssetRipper.SourceGenerated.SourceTpkData", @"D:\Downloads\type_tree.tpk");
    }

    private static void ExportGeneratedData(string typeName, string outputPath)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(typeName, false);
            if (type == null) continue;

            var field = type.GetField("data", BindingFlags.Static | BindingFlags.NonPublic);
            if (field == null) continue;

            var data = (byte[])field.GetValue(null);
            File.WriteAllBytes(outputPath, data);
            break;
        }
    }

}