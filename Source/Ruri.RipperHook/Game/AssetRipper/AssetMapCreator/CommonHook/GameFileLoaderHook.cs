using AssetRipper.GUI.Web;

namespace Ruri.RipperHook.AR_AssetMapCreator;

public partial class AR_AssetMapCreator_Hook
{
    [RetargetMethod(typeof(GameFileLoader), nameof(GameFileLoader.ExportUnityProject), isReturn: false)]
    public static void ExportUnityProject(string path)
    {
        var outputPath = path + $"RuriInfo";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        ExportDictionaryToFile(assetClassIDLookup, Path.Combine(outputPath, "AssetClassIDLookup.txt"));
        ExportDictionaryToFile(assetDependenciesLookup, Path.Combine(outputPath, "AssetDependenciesLookup.txt"));
        ExportDictionaryToFile(assetListLookup, Path.Combine(outputPath, "AssetListLookup.txt"));
    }

    public static void ExportDictionaryToFile<T>(Dictionary<string, HashSet<T>> dictionary, string filePath)
    {
        using (StreamWriter file = new StreamWriter(filePath))
        {
            foreach (var pair in dictionary)
            {
                file.WriteLine($"{pair.Key}:");
                foreach (var item in pair.Value)
                {
                    file.WriteLine($"    {item}");
                }
                file.WriteLine();
            }
        }
    }
}