using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_20;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(typeof(Camera_2019))]
    public void Camera_2019_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Camera_2019;
        var type = typeof(Camera_2019);

        _this.GameObject_C20.ReadRelease(ref reader);
        _this.Enabled_C20 = reader.ReadRelease_ByteAlign();
        _this.ClearFlags_C20 = reader.ReadUInt32();
        _this.BackGroundColor_C20.ReadRelease(ref reader);
        _this.ProjectionMatrixMode_C20 = reader.ReadInt32();
        _this.GateFitMode_C20 = reader.ReadRelease_Int32Align();
        _this.SensorSize_C20.ReadRelease(ref reader);
        _this.LensShift_C20.ReadRelease(ref reader);
        _this.FocalLength_C20 = reader.ReadSingle();
        _this.NormalizedViewPortRect_C20.ReadRelease(ref reader);
        _this.Near_clip_plane_C20 = reader.ReadSingle();
        _this.Far_clip_plane_C20 = reader.ReadSingle();
        _this.Field_of_view_C20 = reader.ReadSingle();
        _this.Orthographic_C20 = reader.ReadRelease_BooleanAlign();
        _this.Orthographic_size_C20 = reader.ReadSingle();
        _this.Depth_C20 = reader.ReadSingle();
        _this.CullingMask_C20.ReadRelease(ref reader);
        _this.RenderingPath_C20 = reader.ReadInt32();
        _this.TargetTexture_C20.ReadRelease(ref reader);
        _this.TargetDisplay_C20_Int32 = reader.ReadInt32();
        _this.TargetEye_C20 = reader.ReadInt32();
        _this.HDR_C20 = reader.ReadBoolean();
        _this.AllowMSAA_C20 = reader.ReadBoolean();
        _this.AllowDynamicResolution_C20 = reader.ReadBoolean();
        _this.ForceIntoRT_C20 = reader.ReadBoolean();
        _this.OcclusionCulling_C20 = reader.ReadBoolean();
        var m_DeformationEnable = reader.ReadRelease_BooleanAlign();
        _this.StereoConvergence_C20 = reader.ReadSingle();
        _this.StereoSeparation_C20 = reader.ReadSingle();
    }
}