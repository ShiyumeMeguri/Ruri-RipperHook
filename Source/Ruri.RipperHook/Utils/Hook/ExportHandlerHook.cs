using AssetRipper.Export.UnityProjects;
using AssetRipper.Processing;
using AssetRipper.Export.UnityProjects.Configuration;
using AssetRipper.Import.Logging;
using AssetRipper.Import.Configuration;
using AssetRipper.Processing.AnimatorControllers;
using AssetRipper.Processing.Assemblies;
using AssetRipper.Processing.AudioMixers;
using AssetRipper.Processing.Editor;
using AssetRipper.Processing.Scenes;
using AssetRipper.Processing.Textures;

namespace Ruri.RipperHook.HookUtils.ExportHandlerHook;

public class ExportHandlerHook : CommonHook
{
    public delegate IEnumerable<IAssetProcessor> AssetProcessorDelegate(LibraryConfiguration Settings);

    public static List<AssetProcessorDelegate> CustomAssetProcessors = new List<AssetProcessorDelegate>();

    // 协程插入代码不现实 PC和state控制地狱级 所以就直接复制源码过来改了
    [RetargetMethod(typeof(ExportHandler), nameof(Process))]
    private void Process(GameData gameData)
    {
        Logger.Info(LogCategory.Processing, "Processing loaded assets...");
        foreach (IAssetProcessor processor in GetProcessors())
        {
            processor.Process(gameData);
        }
        Logger.Info(LogCategory.Processing, "Finished processing assets");
    }
    private IEnumerable<IAssetProcessor> GetProcessors()
    {
        var getMethod = typeof(ExportHandler).GetMethod("get_Settings", ReflectionExtensions.PrivateInstanceBindFlag());
        var Settings = (LibraryConfiguration)getMethod.Invoke(this, null);

        if (Settings.ImportSettings.ScriptContentLevel == ScriptContentLevel.Level1)
        {
            yield return new MethodStubbingProcessor();
        }
        yield return new SceneDefinitionProcessor();
        yield return new MainAssetProcessor();
        yield return new AnimatorControllerProcessor();
        yield return new AudioMixerProcessor();
        yield return new EditorFormatProcessor(Settings.ProcessingSettings.BundledAssetsExportMode);

        // 自定义处理 这里不能利用委托的 += 因为这会导致只接收到最后次返回的值
        foreach (var CustomAssetProcessor in CustomAssetProcessors)
        {
            foreach (var processor in CustomAssetProcessor(Settings))
            {
                yield return processor;
            }
        }

        yield return new LightingDataProcessor();//Needs to be after static mesh separation
        yield return new PrefabProcessor();
        yield return new SpriteProcessor();
    }
}