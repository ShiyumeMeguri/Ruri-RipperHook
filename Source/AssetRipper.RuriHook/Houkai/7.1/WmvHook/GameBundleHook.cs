using AssetRipper.IO.Files.CompressedFiles;
using AssetRipper.IO.Files.ResourceFiles;
using AssetRipper.IO.Files.SerializedFiles;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.Streams.Smart;
using AssetRipper.IO.Files.Streams.MultiFile;
using AssetRipper.Assets.Bundles;
using AssetRipper.IO.Files.SerializedFiles.Parser;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.IO;
using AssetRipper.Primitives;
using System.Reflection;
using AssetRipper.IO.Endian;

namespace AssetRipper.RuriHook.Houkai_7_1;
public sealed class GameBundleHook
{
	private const string FileHeader = "UnityFS\x00";
	private static bool blockXmfInited;
	private static List<WMVInfo> wmwFileInfo;
	private static MethodInfo FromSerializedFile = typeof(SerializedAssetCollection).GetMethod("FromSerializedFile", ReflectionExtension.PrivateStaticBindFlag());
	private static MethodInfo LoadFilesAndDependenciesMethod = typeof(SerializedAssetCollection).GetMethod("LoadFilesAndDependencies", ReflectionExtension.PrivateStaticBindFlag());

	[RetargetMethod(typeof(GameBundle), nameof(InitializeFromPaths), 5)]
	public void InitializeFromPaths(IEnumerable<string> paths, AssetFactoryBase assetFactory, IDependencyProvider? dependencyProvider, IResourceProvider? resourceProvider, UnityVersion defaultVersion = default)
	{
		var _this = (object)this as GameBundle;

		_this.ResourceProvider = resourceProvider;
		List<FileBase> fileStack = new List<FileBase>();
		foreach (var path in paths)
		{
			var extension = Path.GetExtension(path);
			bool isWmv = extension == ".wmv";
			if (!blockXmfInited && isWmv)
			{
				var directory = Path.GetDirectoryName(path);
				var blockXmfPath = Path.Combine(directory, "Blocks.xmf");

				if (File.Exists(blockXmfPath))
				{
					blockXmfInited = true;
					wmwFileInfo = ReadXMF(blockXmfPath);
				}
			}

			if (isWmv)
			{
				var selectedWMVInfo = wmwFileInfo.FirstOrDefault(w => w.FilePath.Equals(path, StringComparison.OrdinalIgnoreCase));
				using (SmartStream stream = SmartStream.OpenRead(path))
				{
					foreach (var asset in selectedWMVInfo.UnitAssetArray)
					{
						byte[] fileData = new byte[asset.FileSize];
						stream.Position = asset.Offset;
						stream.Read(fileData, 0, fileData.Length);

						string filePath = asset.FilePath;
						string directoryPath = Path.GetDirectoryName(filePath);
						string fileName = Path.GetFileName(filePath);

						fileStack.AddRange(LoadFilesAndDependencies(fileData, directoryPath, fileName, dependencyProvider));
					}
				}
			}
			else
			{
				using (SmartStream stream = SmartStream.OpenRead(path))
				{
					byte[] fileData = new byte[stream.Length];
					stream.Read(fileData, 0, fileData.Length);
					fileStack.AddRange(LoadFilesAndDependencies(fileData, MultiFileStream.GetFilePath(path), MultiFileStream.GetFileName(path), dependencyProvider));
				}
			}
		}

		while (fileStack.Count > 0)
		{
			switch (RemoveLastItem(fileStack))
			{
				case SerializedFile serializedFile:
					FromSerializedFile.Invoke(null, new object[] { this, serializedFile, assetFactory, defaultVersion });
					break;
				case FileContainer container:
					SerializedBundle serializedBundle = SerializedBundle.FromFileContainer(container, assetFactory, defaultVersion);
					_this.AddBundle(serializedBundle);
					break;
				case ResourceFile resourceFile:
					_this.AddResource(resourceFile);
					break;
			}
		}
	}
	private static List<WMVInfo> ReadXMF(string filePath)
	{
		List<WMVInfo> list = new List<WMVInfo>();
		using SmartStream stream = SmartStream.OpenRead(filePath);
		using EndianReader reader = new EndianReader(stream, EndianType.BigEndian);

		reader.ReadBytes(16); // Skip 16 bytes
		reader.ReadByte(); // Skip 1 byte
		reader.ReadInt32();
		reader.ReadInt32();
		reader.ReadInt32();
		reader.ReadInt32();
		reader.ReadInt32();
		while (reader.BaseStream.Position < reader.BaseStream.Length - 4)
		{
			WMVInfo wmvInfo = new WMVInfo(); 
			byte[] data = reader.ReadBytes(16);
			string fileName = Convert.ToHexString(data);
			wmvInfo.FilePath = Path.Combine(Path.GetDirectoryName(filePath), fileName + ".wmv");
			reader.ReadInt32();
			reader.ReadInt32();
			reader.ReadInt32();
			wmvInfo.FileSize = reader.ReadInt32();
			wmvInfo.FileCount = reader.ReadInt32();
			WMVInfo.UnitAssetInfo[] assets = new WMVInfo.UnitAssetInfo[wmvInfo.FileCount];

			for (int i = 0; i < wmvInfo.FileCount; i++)
			{
				assets[i] = new WMVInfo.UnitAssetInfo();
				short count = reader.ReadInt16();
				string path = new string(reader.ReadChars(count));
				assets[i].FilePath = Path.Combine(Path.GetDirectoryName(filePath), fileName, path);
				assets[i].Offset = reader.ReadInt32();

				if (i > 0)
				{
					assets[i - 1].FileSize = assets[i].Offset - assets[i - 1].Offset;
				}

				if (i == wmvInfo.FileCount - 1)
				{
					assets[i].FileSize = wmvInfo.FileSize - assets[i].Offset;
				}
			}
			wmvInfo.UnitAssetArray = assets;
			list.Add(wmvInfo);
		}
		return list;
	}
	private static FileBase RemoveLastItem(List<FileBase> list)
	{
		int index = list.Count - 1;
		FileBase file = list[index];
		list.RemoveAt(index);
		return file;
	}
	// 稍微修改了下
	private static List<FileBase> LoadFilesAndDependencies(byte[] buffer, string path, string name, IDependencyProvider? dependencyProvider)
	{
		List<FileBase> files = new();
		HashSet<string> serializedFileNames = new();//Includes missing dependencies
		FileBase? file = SchemeReader.ReadFile(buffer, path, name);
		file?.ReadContentsRecursively();
		while (file is CompressedFile compressedFile)
		{
			file = compressedFile.UncompressedFile;
		}
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
			foreach (SerializedFile serializedFileInContainer in container.FetchSerializedFiles())
			{
				serializedFileNames.Add(serializedFileInContainer.NameFixed);
			}
		}

		for (int i = 0; i < files.Count; i++)
		{
			FileBase file1 = files[i];
			if (file1 is SerializedFile serializedFile)
			{
				LoadDependencies(serializedFile, files, serializedFileNames, dependencyProvider);
			}
			else if (file1 is FileContainer container)
			{
				foreach (SerializedFile serializedFileInContainer in container.FetchSerializedFiles())
				{
					LoadDependencies(serializedFileInContainer, files, serializedFileNames, dependencyProvider);
				}
			}
		}

		return files;
	}

	private static void LoadDependencies(SerializedFile serializedFile, List<FileBase> files, HashSet<string> serializedFileNames, IDependencyProvider? dependencyProvider)
	{
		foreach (FileIdentifier fileIdentifier in serializedFile.Dependencies)
		{
			string name = fileIdentifier.GetFilePath();
			if (serializedFileNames.Add(name) && dependencyProvider?.FindDependency(fileIdentifier) is { } dependency)
			{
				files.Add(dependency);
			}
		}
	}
}
