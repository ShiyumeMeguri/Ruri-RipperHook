using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_74;
using AssetRipper.SourceGenerated.Subclasses.AnimationEvent;
using AssetRipper.SourceGenerated.Subclasses.FloatCurve;
using AssetRipper.SourceGenerated.Subclasses.PPtrCurve;
using AssetRipper.SourceGenerated.Subclasses.QuaternionCurve;
using AssetRipper.SourceGenerated.Subclasses.Vector3Curve;

namespace Ruri.RipperHook.ExAstrisCommon;

public partial class ExAstrisCommon_Hook
{
    [RetargetMethod(typeof(AnimationClip_2018_3))]
    public void AnimationClip_2018_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as AnimationClip_2018_3;
        var type = typeof(AnimationClip_2018_3);

        _this.Name = reader.ReadRelease_Utf8StringAlign();
        _this.Legacy_C74 = reader.ReadBoolean();
        _this.Compressed_C74 = reader.ReadBoolean();
        _this.Compressed_C74 = false; // Hack
        _this.UseHighQualityCurve_C74 = reader.ReadRelease_BooleanAlign();
        SetAssetListField<QuaternionCurve_2018>(type, "m_RotationCurves", ref reader);
        _this.CompressedRotationCurves_C74.ReadRelease_ArrayAlign_Asset(ref reader);
        var m_aclType = reader.ReadInt32();
        SetAssetListField<Vector3Curve_2018>(type, "m_EulerCurves", ref reader);
        SetAssetListField<Vector3Curve_2018>(type, "m_PositionCurves", ref reader);
        SetAssetListField<Vector3Curve_2018>(type, "m_ScaleCurves", ref reader);
        SetAssetListField<FloatCurve_2018>(type, "m_FloatCurves", ref reader);
        SetAssetListField<PPtrCurve_2017>(type, "m_PPtrCurves", ref reader);
        _this.SampleRate_C74 = reader.ReadSingle();
        _this.WrapMode_C74 = reader.ReadInt32();
        _this.Bounds_C74.ReadRelease(ref reader);
        _this.MuscleClipSize_C74 = reader.ReadUInt32();
        _this.MuscleClip_C74.ReadRelease(ref reader);
        _this.ClipBindingConstant_C74.ReadRelease(ref reader);
        _this.HasGenericRootTransform_C74 = reader.ReadBoolean();
        _this.HasMotionFloatCurves_C74 = reader.ReadRelease_BooleanAlign();
        SetAssetListField<AnimationEvent_5>(type, "m_Events", ref reader);
    }
}