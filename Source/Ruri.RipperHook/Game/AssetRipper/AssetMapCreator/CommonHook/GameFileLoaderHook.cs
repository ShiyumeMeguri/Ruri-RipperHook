using AssetRipper.GUI.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ruri.RipperHook.AR_AssetMapCreator;

public partial class AR_AssetMapCreator_Hook
{
    [RetargetMethod(typeof(GameFileLoader), nameof(GameFileLoader.ExportUnityProject), isReturn: false)]
    public static void ExportUnityProject(string path)
    {
        var outputPath = path + $"RuriInfo"; // 放里面会被后处理清空 时机问题
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        // 修改文件扩展名为 .json 更直观
        ExportDictionaryToFile(assetClassIDLookup, Path.Combine(outputPath, "AssetClassIDLookup.json"));
        ExportDictionaryToFile(assetDependenciesLookup, Path.Combine(outputPath, "AssetDependenciesLookup.json"));
        ExportDictionaryToFile(assetListLookup, Path.Combine(outputPath, "AssetListLookup.json"));
    }

    public static void ExportDictionaryToFile<T>(Dictionary<string, HashSet<T>> dictionary, string filePath)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            // 使用 StringEnumConverter 使枚举以字符串形式导出
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
        // 序列化整个字典为 JSON 字符串
        string json = JsonConvert.SerializeObject(dictionary, settings);
        File.WriteAllText(filePath, json);
    }
}
