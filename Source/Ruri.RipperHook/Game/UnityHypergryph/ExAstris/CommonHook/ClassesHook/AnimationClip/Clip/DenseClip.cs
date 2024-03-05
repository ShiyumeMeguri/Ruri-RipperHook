using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Extensions;
using AssetRipper.SourceGenerated.Subclasses.DenseClip;

namespace Ruri.RipperHook.ExAstrisCommon;

public partial class ExAstrisCommon_Hook
{
    private static int m_ACLType;
    private static uint m_nPositionCurves;
    private static uint m_nRotationCurves;
    private static uint m_nEulerCurves;
    private static uint m_nScaleCurves;
    private static uint m_nGenericCurves;
    private static float m_PositionFactor;
    private static float m_EulerFactor;
    private static float m_ScaleFactor;
    private static float m_FloatFactor;
    private static byte[] m_ACLArray;

    [RetargetMethod(typeof(DenseClip))]
    public void DenseClip_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as DenseClip;
        var type = typeof(DenseClip);

        _this.FrameCount = reader.ReadInt32();
        _this.CurveCount = reader.ReadUInt32();
        _this.SampleRate = reader.ReadSingle();
        _this.BeginTime = reader.ReadSingle();
        _this.SampleArray.ReadRelease_Array_Single(ref reader);

        // ExAstris Add
        m_ACLType = reader.ReadInt32();
        m_nPositionCurves = reader.ReadUInt32();
        m_nRotationCurves = reader.ReadUInt32();
        m_nEulerCurves = reader.ReadUInt32();
        m_nScaleCurves = reader.ReadUInt32();
        m_nGenericCurves = reader.ReadUInt32();
        m_PositionFactor = reader.ReadSingle();
        m_EulerFactor = reader.ReadSingle();
        m_ScaleFactor = reader.ReadSingle();
        m_FloatFactor = reader.ReadSingle();
        m_ACLArray = reader.ReadRelease_ArrayAlign_Byte();

        MergeAclClip(_this, type);
    }
    private void MergeAclClip(DenseClip _this, Type type)
    {
        if (m_ACLType == 0 || !_this.SampleArray.ToArray().IsNullOrEmpty()) return;

        var sampleArray = new List<float>();

        var size = m_ACLType >> 2;
        var factor = (float)((1 << m_ACLType) - 1);
        var aclSpan = m_ACLArray.ToUInt4Array().AsSpan();
        var buffer = (stackalloc byte[8]);

        for (int i = 0; i < _this.FrameCount; i++)
        {
            var index = i * (int)(_this.CurveCount * size);
            for (int j = 0; j < m_nPositionCurves; j++)
            {
                sampleArray.Add(ReadCurve(aclSpan, m_PositionFactor, ref index));
            }
            for (int j = 0; j < m_nRotationCurves; j++)
            {
                sampleArray.Add(ReadCurve(aclSpan, 1.0f, ref index));
            }
            for (int j = 0; j < m_nEulerCurves; j++)
            {
                sampleArray.Add(ReadCurve(aclSpan, m_EulerFactor, ref index));
            }
            for (int j = 0; j < m_nScaleCurves; j++)
            {
                sampleArray.Add(ReadCurve(aclSpan, m_ScaleFactor, ref index));
            }
            var m_nFloatCurves = _this.CurveCount - (m_nPositionCurves + m_nRotationCurves + m_nEulerCurves + m_nScaleCurves + m_nGenericCurves);
            for (int j = 0; j < m_nFloatCurves; j++)
            {
                sampleArray.Add(ReadCurve(aclSpan, m_FloatFactor, ref index));
            }
        }

        AssetList<float> m_SampleArray = [.. sampleArray]; 
        SetPrivateField(type, "m_SampleArray", m_SampleArray);
    }

    private float ReadCurve(Span<byte> aclSpan, float curveFactor, ref int curveIndex)
    {
        var buffer = (stackalloc byte[8]);

        var curveSize = m_ACLType >> 2;
        var factor = (float)((1 << m_ACLType) - 1);

        aclSpan.Slice(curveIndex, curveSize).CopyTo(buffer);
        var temp = buffer.ToArray().ToUInt8Array(0, curveSize);
        buffer.Clear();
        temp.CopyTo(buffer);

        float curve;
        var value = BitConverter.ToUInt64(buffer);
        if (value != 0)
        {
            curve = ((value / factor) - 0.5f) * 2;
        }
        else
        {
            curve = -1.0f;
        }

        curve *= curveFactor;
        curveIndex += curveSize;

        return curve;
    }
}