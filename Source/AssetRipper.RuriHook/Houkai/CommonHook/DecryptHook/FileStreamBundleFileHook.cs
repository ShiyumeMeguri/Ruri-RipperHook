using AssetRipper.IO.Files.BundleFiles.FileStream;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.RuriHook.Crypto;
using K4os.Compression.LZ4;
using System.Reflection;
using AssetRipper.IO.Files.Extensions;
using AssetRipper.IO.Files.Exceptions;

namespace AssetRipper.RuriHook.HoukaiCommon
{
	public partial class HoukaiCommon_Hook
	{
		private static MethodInfo ReadMetadata = typeof(FileStreamBundleFile).GetMethod("ReadMetadata", ReflectionExtension.PrivateInstanceBindFlag());

		[RetargetMethod(typeof(FileStreamBundleFile), nameof(ReadFileStreamMetadata), 2)]
		public void ReadFileStreamMetadata(Stream stream, long basePosition)
		{
			var _this = (object)this as FileStreamBundleFile;
			
			var Header = _this.Header;
			var NameFixed = _this.NameFixed;
			if (Header.Version >= BundleVersion.BF_LargeFilesSupport)
			{
				stream.Align(16);
			}
			if (Header.Flags.GetBlocksInfoAtTheEnd())
			{
				Console.WriteLine(NameFixed);
				stream.Position = basePosition + (Header.Size - Header.CompressedBlocksInfoSize);
			}

			CompressionType metaCompression = Header.Flags.GetCompression();
			byte[] compressedBytes = new BinaryReader(stream).ReadBytes(Header.CompressedBlocksInfoSize);
			switch (metaCompression)
			{
				case CompressionType.None:
					{
						ReadMetadata.Invoke(this, new object[] { stream, Header.UncompressedBlocksInfoSize });
					}
					break;

				case CompressionType.Lzma:
					{
						using MemoryStream uncompressedStream = new MemoryStream(new byte[Header.UncompressedBlocksInfoSize]);
						LzmaCompression.DecompressLzmaStream(stream, Header.CompressedBlocksInfoSize, uncompressedStream, Header.UncompressedBlocksInfoSize);

						uncompressedStream.Position = 0;
						ReadMetadata.Invoke(this, new object[] { uncompressedStream, Header.UncompressedBlocksInfoSize });
					}
					break;

				case CompressionType.Lz4:
				case CompressionType.Lz4HC:
					{
						int uncompressedSize = Header.UncompressedBlocksInfoSize;
						byte[] uncompressedBytes = new byte[uncompressedSize];
						int bytesWritten = LZ4Codec.Decode(compressedBytes, uncompressedBytes);
						if (bytesWritten < 0)
						{
							Console.WriteLine("EncryptedFileException.Throw(NameFixed)");
						}
						else if (bytesWritten != uncompressedSize)
						{
							Console.WriteLine("DecompressionFailedException.ThrowIncorrectNumberBytesWritten(NameFixed, uncompressedSize, bytesWritten)");
						}
						ReadMetadata.Invoke(this, new object[] { new MemoryStream(uncompressedBytes), uncompressedSize });
					}
					break;

				case (CompressionType)5:
					if (Mr0kUtils.IsMr0k(compressedBytes))
					{
						compressedBytes = Mr0kUtils.Decrypt(compressedBytes, (Mr0k)RuriRuntimeHook.gameCrypto).ToArray();
					}
					goto case CompressionType.Lz4HC;

				default:
					UnsupportedBundleDecompression.Throw(NameFixed, metaCompression);
					break;
			}
		}
	}
}
