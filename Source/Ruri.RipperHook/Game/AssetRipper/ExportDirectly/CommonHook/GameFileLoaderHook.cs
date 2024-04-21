using AssetRipper.GUI.Web;
using AssetRipper.Import.Logging;
using System.IO;

namespace Ruri.RipperHook.AR_ExportDirectly;

public partial class AR_ExportDirectly_Hook
{

    [RetargetMethod(typeof(GameFileLoader), nameof(LoadAndProcess), 0, isBefore:false, isReturn:false)]
    public static void LoadAndProcess(IReadOnlyList<string> paths)
    {
        var directoryPath = Path.GetDirectoryName(paths[0]);
        var fileName = Path.GetFileNameWithoutExtension(paths[0]);
        var outputPath = Path.Combine(directoryPath, $"{fileName}Output");
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        Logger.Info($"直接导出到了 : {outputPath}");
        GameFileLoader.Export(outputPath);
    }
}