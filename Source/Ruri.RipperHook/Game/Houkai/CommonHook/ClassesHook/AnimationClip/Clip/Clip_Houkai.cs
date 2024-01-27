using System.Runtime.CompilerServices;
using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.Clip;

namespace Ruri.RipperHook.HoukaiCommon;

public partial class HoukaiCommon_Hook
{
    [RetargetMethod(typeof(Clip_5_5_0))]
    public void Clip_5_5_0_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Clip_5_5_0;
        var type = typeof(Clip_5_5_0);

        var currentPosition = reader.Position; // 预记录还原读取位置

        reader.Position += (reader.ReadInt32() + 1) * sizeof(uint);
        _this.StreamedClip.CurveCount = reader.ReadUInt32();
        _this.DenseClip.ReadRelease(ref reader);
        _this.ConstantClip.ReadRelease(ref reader);
        // Acl Process
        var aclClipData = reader.ReadRelease_ArrayAlign_Byte();
        var aclCurveCount = reader.ReadUInt32();
        _this.StreamedClip.CurveCount += aclCurveCount;
        _this.Binding.ReadRelease(ref reader);
        if (aclCurveCount == 0) return;

        var endPosition = reader.Position;

        reader.Position = currentPosition;
        MergeAclClip(ref reader, aclClipData, _this.StreamedClip.Data, aclCurveCount);
        reader.Position = endPosition;
    }

    private static unsafe void MergeAclClip(ref EndianSpanReader reader,
        byte[] aclClipData,
        AssetList<uint> data,
        uint aclCurveCount)
    {
        AclHook.DecompressAll(aclClipData, out var curveValues, out var aclTimes);
        var streamedIndex = 0;
        var aclFrameIndex = 0;
        var streamedCount = reader.ReadInt32();
        // Pre Process Start Frame
        {
            var time = reader.ReadSingle();
            var count = reader.ReadUInt32();
            data.Add(*(uint*)&time); // Time
            data.Add(*&count + aclCurveCount); // Count
            ProcessAcl(data, curveValues, time, aclCurveCount, 0);
            ProcessStreamed(ref reader, ref streamedIndex, data, count, aclCurveCount);
        }
        while (streamedIndex < streamedCount)
        {
            var aclTime = aclTimes[aclFrameIndex];
            var time = reader.ReadSingle();
            streamedIndex++;
            var count = reader.ReadUInt32();
            streamedIndex++;
            while (aclTime - time < -0.001f && aclFrameIndex < aclTimes.Length - 1 && !float.IsInfinity(time))
            {
                data.Add(*(uint*)&time); // Time
                data.Add(*&aclCurveCount); // Count
                ProcessAcl(data, curveValues, aclTime, aclCurveCount, aclFrameIndex);
                aclFrameIndex++;
                aclTime = aclTimes[aclFrameIndex];
            }

            data.Add(*(uint*)&time); // Time
            data.Add(*&count); // Count
            ProcessStreamed(ref reader, ref streamedIndex, data, count, aclCurveCount);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void ProcessAcl(AssetList<uint> data,
        float[] curveValues,
        float time,
        uint aclCurveCount,
        int aclFrameIndex)
    {
        var offset = (uint)(aclFrameIndex * aclCurveCount);
        for (var curveIndex = 0; curveIndex < aclCurveCount;)
        {
            var framePosition = (int)(offset + curveIndex);
            var value = curveValues[framePosition];

            data.Add(*(uint*)&curveIndex); // AclIndex
            data.Add(0); // AclCoefficient X
            data.Add(0); // AclCoefficient Y
            data.Add(0); // AclCoefficient Z
            data.Add(*(uint*)&value); // AclValue
            curveIndex++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessStreamed(ref EndianSpanReader reader,
        ref int streamedIndex,
        AssetList<uint> data,
        uint count,
        uint aclCurveCount)
    {
        for (var j = 0; j < count; j++)
        {
            data.Add(reader.ReadUInt32() + aclCurveCount);
            streamedIndex++; // StreamedIndex
            data.Add(reader.ReadUInt32());
            streamedIndex++; // StreamedCoefficient X
            data.Add(reader.ReadUInt32());
            streamedIndex++; // StreamedCoefficient Y
            data.Add(reader.ReadUInt32());
            streamedIndex++; // StreamedCoefficient Z
            data.Add(reader.ReadUInt32());
            streamedIndex++; // StreamedValue
        }
    }
}