using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.BundleFiles;
using AssetRipper.RuriHook.Crypto;
using AssetRipper.IO.Files.BundleFiles.FileStream;

namespace AssetRipper.RuriHook.HoukaiCommon;
public partial class HoukaiCommon_Hook
{
	[RetargetMethod(typeof(FileStreamBundleHeader), nameof(Read))]
	public void Read(EndianReader reader)
	{
		var _this = (object)this as FileStreamBundleHeader;

		string signature = reader.ReadStringZeroTerm();
		var key = reader.ReadUInt32();
		if (key <= 11)
		{
			reader.BaseStream.Position -= 4;
			_this.Version = (BundleVersion)reader.ReadInt32();
			_this.UnityWebBundleVersion = reader.ReadStringZeroTerm();
			_this.UnityWebMinimumRevision = reader.ReadStringZeroTerm();
		}
		XORShift128.InitSeed(key);

		_this.Version = BundleVersion.BF_520_x;
		_this.UnityWebBundleVersion = "5.x.x";
		_this.UnityWebMinimumRevision = "2017.4.18f1";
		if (XORShift128.Init)
		{
			_this.Flags = (BundleFlags)(reader.ReadUInt32() ^ XORShift128.NextDecryptInt());
			_this.Size = reader.ReadInt64() ^ XORShift128.NextDecryptLong();
			_this.UncompressedBlocksInfoSize = (int)(reader.ReadUInt32() ^ XORShift128.NextDecryptUInt());
			_this.CompressedBlocksInfoSize = (int)(reader.ReadUInt32() ^ XORShift128.NextDecryptUInt());

			XORShift128.Init = false;

			var encUnityVersion = reader.ReadStringZeroTerm();
			var encUnityRevision = reader.ReadStringZeroTerm();
			return;
		}

		_this.Size = reader.ReadInt64();
		_this.CompressedBlocksInfoSize = reader.ReadInt32();
		_this.UncompressedBlocksInfoSize = reader.ReadInt32();
		_this.Flags = (BundleFlags)reader.ReadInt32();
	}
}
