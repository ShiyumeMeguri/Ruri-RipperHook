using AssetRipper.Assets.Bundles;
using AssetRipper.IO.Endian;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.IO.Files.Streams.Smart;
using Ruri.RipperHook.HookUtils.GameBundleHook;

namespace Ruri.RipperHook.HoukaiCommon;

public partial class HoukaiCommon_Hook
{
    private static bool blockXmfInited;
    private static List<WMVInfo> wmwFileInfo;

    public static void CustomFilePreInitialize(GameBundle _this, IEnumerable<string> paths, List<FileBase> fileStack, IDependencyProvider? dependencyProvider)
    {
        foreach (var path in paths)
        {
            var extension = Path.GetExtension(path);
            var isWmv = extension == ".wmv";
            if (!blockXmfInited && isWmv)
            {
                var wmwPath = Path.GetDirectoryName(path);
                var blockXmfPath = Path.Combine(wmwPath, "Blocks.xmf");

                if (!File.Exists(blockXmfPath))
                    blockXmfPath = Path.Combine("Game", RuriRuntimeHook.gameName, RuriRuntimeHook.gameVer, "Blocks.xmf");
                if (File.Exists(blockXmfPath))
                {
                    blockXmfInited = true;
                    wmwFileInfo = ReadXMF(blockXmfPath, wmwPath);
                }
                else 
                {
                    throw new Exception($"把Blocks.xmf放到wmw所在目录下或者程序根目录 {Directory.GetCurrentDirectory()}/Game/{RuriRuntimeHook.gameName}/{RuriRuntimeHook.gameVer}/ 下");
                }
            }

            if (isWmv)
            {
                var selectedWMVInfo = wmwFileInfo.FirstOrDefault(w => w.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase));
                using (var stream = SmartStream.OpenRead(path))
                {
                    foreach (var asset in selectedWMVInfo.UnitAssetArray)
                    {
                        var fileData = new byte[asset.FileSize];
                        stream.Position = asset.Offset;
                        stream.Read(fileData, 0, fileData.Length);

                        var filePath = asset.FilePath;
                        var directoryPath = Path.GetDirectoryName(filePath);
                        var fileName = Path.GetFileName(filePath);

                        fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(fileData, directoryPath, fileName, dependencyProvider));
                    }
                }
            }
            else
            {
                using (var stream = SmartStream.OpenRead(path))
                {
                    var fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);
                    fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path),
                        MultiFileStream.GetFileName(path), dependencyProvider));
                }
            }
        }
    }

    private static List<WMVInfo> ReadXMF(string xmfPath, string wmwPath)
    {
        var list = new List<WMVInfo>();
        using var stream = SmartStream.OpenRead(xmfPath);
        using var reader = new EndianReader(stream, EndianType.BigEndian);

        reader.ReadBytes(16); // Skip 16 bytes
        reader.ReadByte(); // Skip 1 byte
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        while (reader.BaseStream.Position < reader.BaseStream.Length - 4)
        {
            var wmvInfo = new WMVInfo();
            var data = reader.ReadBytes(16);
            var fileName = Convert.ToHexString(data);
            wmvInfo.FilePath = Path.Combine(wmwPath, fileName + ".wmv");
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadInt32();
            wmvInfo.FileSize = reader.ReadInt32();
            wmvInfo.FileCount = reader.ReadInt32();
            var assets = new WMVInfo.UnitAssetInfo[wmvInfo.FileCount];

            for (var i = 0; i < wmvInfo.FileCount; i++)
            {
                assets[i] = new WMVInfo.UnitAssetInfo();
                var count = reader.ReadInt16();
                var path = new string(reader.ReadChars(count));
                assets[i].FilePath = Path.Combine(path, fileName, path);
                assets[i].Offset = reader.ReadInt32();

                if (i > 0) assets[i - 1].FileSize = assets[i].Offset - assets[i - 1].Offset;

                if (i == wmvInfo.FileCount - 1) assets[i].FileSize = wmvInfo.FileSize - assets[i].Offset;
            }

            wmvInfo.UnitAssetArray = assets;
            list.Add(wmvInfo);
        }

        return list;
    }
}