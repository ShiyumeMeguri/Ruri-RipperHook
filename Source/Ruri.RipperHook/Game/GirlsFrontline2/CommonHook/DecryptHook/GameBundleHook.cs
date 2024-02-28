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

namespace Ruri.RipperHook.GirlsFrontline2Common;

public sealed class GameBundleHook
{
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
            using (var stream = SmartStream.OpenRead(path))
            {
                var fileData = new byte[stream.Length];
                stream.Read(fileData, 0, fileData.Length);

                if (fileData.Length >= 4 && fileData[0] == 0x47 && fileData[1] == 0x46 && fileData[2] == 0x46 && fileData[3] == 0x00)
                    continue; // 这应该是索引文件

                fileData = RuriRuntimeHook.commonDecryptor.Decrypt(fileData).ToArray(); // 解密处
                fileStack.AddRange(LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path),
                    MultiFileStream.GetFileName(path), dependencyProvider));
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