using AssetRipper.Assets.Bundles;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.IO.Files.Streams.Smart;
using Ruri.RipperHook.HookUtils.GameBundleHook;

namespace Ruri.RipperHook.GirlsFrontline2Common;

public partial class GirlsFrontline2Common_Hook
{
    public static void CustomFilePreInitialize(GameBundle _this, IEnumerable<string> paths, List<FileBase> fileStack, IDependencyProvider? dependencyProvider)
    {
        foreach (var path in paths)
        {
            using (var stream = SmartStream.OpenRead(path))
            {
                var fileData = new byte[stream.Length];
                stream.Read(fileData, 0, fileData.Length);

                if (fileData.Length >= 4 && fileData[0] == 0x47 && fileData[1] == 0x46 && fileData[2] == 0x46 && fileData[3] == 0x00)
                    continue; // 文件头 "GFF" 这应该是索引文件

                fileData = RuriRuntimeHook.commonDecryptor.Decrypt(fileData).ToArray(); // 解密处
                fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path), MultiFileStream.GetFileName(path), dependencyProvider));
            }
        }
    }
}