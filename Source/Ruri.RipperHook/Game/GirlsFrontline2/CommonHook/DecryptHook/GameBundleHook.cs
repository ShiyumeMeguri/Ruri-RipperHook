using AssetRipper.Assets.Bundles;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.IO.Files.Streams.Smart;
using Ruri.RipperHook.HookUtils.GameBundleHook;

namespace Ruri.RipperHook.GirlsFrontline2Common;

public partial class GirlsFrontline2Common_Hook
{
    public static readonly byte[] dataHead = { 0x47, 0x46, 0x46, 0x00 };
    public static readonly byte[] unityFSHead = { 0x55, 0x6E, 0x69, 0x74, 0x79, 0x46, 0x53, 0x00 };
    public static void CustomFilePreInitialize(GameBundle _this, IEnumerable<string> paths, List<FileBase> fileStack, IDependencyProvider? dependencyProvider)
    {
        foreach (var path in paths)
        {
            var extension = Path.GetExtension(path);
            using (var stream = SmartStream.OpenRead(path))
            {
                var fileData = new byte[stream.Length];
                stream.Read(fileData, 0, fileData.Length);

                if (fileData.StartsWith(dataHead))
                    continue; // 文件头 "GFF" 这应该是数据文件
                
                if (extension == ".bundle")
                    if (!fileData.StartsWith(unityFSHead))
                        fileData = RuriRuntimeHook.commonDecryptor.Decrypt(fileData).ToArray(); // 解密处

                fileStack.AddRange(GameBundleHook.LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path), MultiFileStream.GetFileName(path), dependencyProvider));
            }
        }
    }
}