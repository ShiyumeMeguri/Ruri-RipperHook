using AssetRipper.Assets.Bundles;
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
                    var assetBundleBlocks = MihoyoCommon.FindBlockFiles(stream, encrHead);
                    for (int i = 0;i<assetBundleBlocks.Count;i++)
                    {
                        var filePath = path;
                        var directoryPath = Path.GetDirectoryName(filePath);
                        var fileName = Path.GetFileName(filePath);

                        fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(assetBundleBlocks[i], directoryPath, fileName, dependencyProvider));
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