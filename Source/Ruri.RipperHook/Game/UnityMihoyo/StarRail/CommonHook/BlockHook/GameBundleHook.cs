using AssetRipper.Assets.Bundles;
using AssetRipper.Import.Logging;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.IO.Files.Streams.Smart;
using Ruri.RipperHook.HookUtils.GameBundleHook;
using Ruri.RipperHook.UnityMihoyo;

namespace Ruri.RipperHook.StarRailCommon;

public partial class StarRailCommon_Hook
{
    public static readonly byte[] encrHead = { 0x45, 0x4E, 0x43, 0x52, 0x00 };
    public static void CustomFilePreInitialize(GameBundle _this, IEnumerable<string> paths, List<FileBase> fileStack, IDependencyProvider? dependencyProvider)
    {
        foreach (var path in paths)
        {
            var extension = Path.GetExtension(path);

            if (extension == ".block")
            {
                using (var stream = SmartStream.OpenRead(path))
                {
                    var assets = MihoyoCommon.FindBlockFiles(stream, encrHead, path);
                    foreach (var asset in assets)
                    {
                        Logger.Info($"找到Block {asset.FilePath}");

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
                    fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path), MultiFileStream.GetFileName(path), dependencyProvider));
                }
            }
        }
    }
}