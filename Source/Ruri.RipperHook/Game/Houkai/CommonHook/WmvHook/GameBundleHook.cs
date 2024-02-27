using System.Reflection;
using AssetRipper.Assets.Bundles;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.IO;
using AssetRipper.IO.Endian;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.CompressedFiles;
using AssetRipper.IO.Files.ResourceFiles;
using AssetRipper.IO.Files.SerializedFiles;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.IO.Files.Streams.Smart;
using AssetRipper.Primitives;

namespace Ruri.RipperHook.HoukaiCommon;

public sealed class GameBundleHook
{
    private static bool blockXmfInited;
    private static List<WMVInfo> wmwFileInfo;

    private static readonly MethodInfo FromSerializedFile =
        typeof(SerializedAssetCollection).GetMethod("FromSerializedFile", ReflectionExtensions.PrivateStaticBindFlag());

    [RetargetMethod(typeof(GameBundle), nameof(InitializeFromPaths), 5)]
    public void InitializeFromPaths(IEnumerable<string> paths, AssetFactoryBase assetFactory, IDependencyProvider? dependencyProvider, IResourceProvider? resourceProvider, UnityVersion defaultVersion = default)
    {
        var _this = (object)this as GameBundle;

        _this.ResourceProvider = resourceProvider;
        var fileStack = new List<FileBase>();
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

                        fileStack.AddRange(LoadFilesAndDependencies(fileData, directoryPath, fileName,
                            dependencyProvider));
                    }
                }
            }
            else
            {
                using (var stream = SmartStream.OpenRead(path))
                {
                    var fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);
                    fileStack.AddRange(LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path),
                        MultiFileStream.GetFileName(path), dependencyProvider));
                }
            }
        }

        while (fileStack.Count > 0)
            switch (RemoveLastItem(fileStack))
            {
                case SerializedFile serializedFile:
                    FromSerializedFile.Invoke(null, new object[] { this, serializedFile, assetFactory, defaultVersion });
                    break;
                case FileContainer container:
                    var serializedBundle = SerializedBundle.FromFileContainer(container, assetFactory, defaultVersion);
                    _this.AddBundle(serializedBundle);
                    break;
                case ResourceFile resourceFile:
                    _this.AddResource(resourceFile);
                    break;
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

    private static FileBase RemoveLastItem(List<FileBase> list)
    {
        var index = list.Count - 1;
        var file = list[index];
        list.RemoveAt(index);
        return file;
    }

    // 稍微修改了下
    private static List<FileBase> LoadFilesAndDependencies(byte[] buffer,
        string path,
        string name,
        IDependencyProvider? dependencyProvider)
    {
        List<FileBase> files = new();
        HashSet<string> serializedFileNames = new(); //Includes missing dependencies
        var file = SchemeReader.ReadFile(buffer, path, name);
        file?.ReadContentsRecursively();
        while (file is CompressedFile compressedFile) file = compressedFile.UncompressedFile;
        if (file is ResourceFile resourceFile)
        {
            files.Add(file);
        }
        else if (file is SerializedFile serializedFile)
        {
            files.Add(file);
            serializedFileNames.Add(serializedFile.NameFixed);
        }
        else if (file is FileContainer container)
        {
            files.Add(file);
            foreach (var serializedFileInContainer in container.FetchSerializedFiles())
                serializedFileNames.Add(serializedFileInContainer.NameFixed);
        }

        for (var i = 0; i < files.Count; i++)
        {
            var file1 = files[i];
            if (file1 is SerializedFile serializedFile)
                LoadDependencies(serializedFile, files, serializedFileNames, dependencyProvider);
            else if (file1 is FileContainer container)
                foreach (var serializedFileInContainer in container.FetchSerializedFiles())
                    LoadDependencies(serializedFileInContainer, files, serializedFileNames, dependencyProvider);
        }

        return files;
    }

    private static void LoadDependencies(SerializedFile serializedFile,
        List<FileBase> files,
        HashSet<string> serializedFileNames,
        IDependencyProvider? dependencyProvider)
    {
        foreach (var fileIdentifier in serializedFile.Dependencies)
        {
            var name = fileIdentifier.GetFilePath();
            if (serializedFileNames.Add(name) && dependencyProvider?.FindDependency(fileIdentifier) is { } dependency)
                files.Add(dependency);
        }
    }
}