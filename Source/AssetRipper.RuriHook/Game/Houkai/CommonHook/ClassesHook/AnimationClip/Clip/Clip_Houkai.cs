using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.RuriHook.HoukaiCommon;
using AssetRipper.SourceGenerated.Subclasses.Clip;
using System.Runtime.CompilerServices;

namespace AssetRipper.RuriHook.HoukaiCommon;
public partial class HoukaiCommon_Hook
{
	[RetargetMethod(typeof(Clip_5_5_0))]
	public unsafe void Clip_5_5_0_ReadRelease(ref EndianSpanReader reader)
	{
		var _this = (object)this as Clip_5_5_0;
		var type = typeof(Clip_5_5_0);

		var currentPosition = reader.Position; // 预记录还原读取位置

		reader.Position += (reader.ReadInt32() + 1) * sizeof(uint);
		_this.StreamedClip.CurveCount = reader.ReadUInt32();
		_this.DenseClip.ReadRelease(ref reader);
		_this.ConstantClip.ReadRelease(ref reader);
		// Acl Process
		byte[] aclClipData = reader.ReadRelease_ArrayAlign_Byte();
		uint aclCurveCount = reader.ReadUInt32();
		_this.StreamedClip.CurveCount += aclCurveCount;
		_this.Binding.ReadRelease(ref reader);
		if (aclCurveCount == 0) return;

		var endPosition = reader.Position;

		reader.Position = currentPosition;
		AssetList<uint> data = _this.StreamedClip.Data;
		MergeAclClip(ref reader, aclClipData, data, aclCurveCount);
		reader.Position = endPosition;
	}
	private static unsafe void MergeAclClip(ref EndianSpanReader reader, byte[] aclClipData, AssetList<uint> data, uint aclCurveCount)
	{
		AclHook.DecompressAll(aclClipData, out float[]? curveValues, out float[]? aclTimes);
		int streamedIndex = 0;
		int aclFrameIndex = 0;
		int streamedCount = reader.ReadInt32();
		// Pre Process Start Frame
		{
			float time = reader.ReadSingle();
			uint count = reader.ReadUInt32();
			data.Add(*(uint*)&time); // Time
			data.Add(*(uint*)&count + aclCurveCount); // Count
			ProcessAcl(data, curveValues, time, aclCurveCount, 0);
			ProcessStreamed(ref reader, ref streamedIndex, data, count, aclCurveCount);
		}
		while (streamedIndex < streamedCount)
		{
			float aclTime = aclTimes[aclFrameIndex];
			float time = reader.ReadSingle(); streamedIndex++;
			uint count = reader.ReadUInt32(); streamedIndex++;
			while ((aclTime - time) < -0.001f && aclFrameIndex < aclTimes.Length - 1 && !float.IsInfinity(time))
			{
				data.Add(*(uint*)&time); // Time
				data.Add(*(uint*)&aclCurveCount); // Count
				ProcessAcl(data, curveValues, aclTime, aclCurveCount, aclFrameIndex);
				aclFrameIndex++;
				aclTime = aclTimes[aclFrameIndex];
			}
			data.Add(*(uint*)&time); // Time
			data.Add(*(uint*)&count); // Count
			ProcessStreamed(ref reader, ref streamedIndex, data, count, aclCurveCount);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void ProcessAcl(AssetList<uint> data, float[] curveValues, float time, uint aclCurveCount, int aclFrameIndex)
	{
		uint offset = (uint)(aclFrameIndex * aclCurveCount);
		for (int curveIndex = 0; curveIndex < aclCurveCount;)
		{
			int framePosition = (int)(offset + curveIndex);
			float value = curveValues[framePosition];

			data.Add(*(uint*)&curveIndex); // AclIndex
			data.Add(0); // AclCoefficient X
			data.Add(0); // AclCoefficient Y
			data.Add(0); // AclCoefficient Z
			data.Add(*(uint*)&value); // AclValue
			curveIndex++;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void ProcessStreamed(ref EndianSpanReader reader, ref int streamedIndex, AssetList<uint> data, uint count, uint aclCurveCount)
	{
		for (int j = 0; j < count; j++)
		{
			data.Add(reader.ReadUInt32() + aclCurveCount); streamedIndex++; // StreamedIndex
			data.Add(reader.ReadUInt32()); streamedIndex++; // StreamedCoefficient X
			data.Add(reader.ReadUInt32()); streamedIndex++; // StreamedCoefficient Y
			data.Add(reader.ReadUInt32()); streamedIndex++; // StreamedCoefficient Z
			data.Add(reader.ReadUInt32()); streamedIndex++; // StreamedValue
		}
	}
}
