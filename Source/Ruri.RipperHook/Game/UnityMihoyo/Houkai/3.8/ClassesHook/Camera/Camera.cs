using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_20;

namespace Ruri.RipperHook.Houkai_3_8;

public partial class Houkai_3_8_Hook
{
    [RetargetMethod(typeof(Camera_2017_3))]
    public void Camera_2017_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Camera_2017_3;
        var type = typeof(Camera_2017_3);

        _this.GameObject_C20.ReadRelease(ref reader);
        _this.Enabled_C20 = reader.ReadRelease_ByteAlign();
        _this.ClearFlags_C20 = reader.ReadUInt32();
        _this.BackGroundColor_C20.ReadRelease(ref reader);
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
        _this.OcclusionCulling_C20 = reader.ReadRelease_BooleanAlign();
        _this.StereoConvergence_C20 = reader.ReadSingle();
        _this.StereoSeparation_C20 = reader.ReadSingle();
        var HalfResolutionParticleEnable = reader.ReadBoolean();
        var OutputHalfResolutionDepth = reader.ReadBoolean();
        var MRTOutputDepthNormal = reader.ReadBoolean();
        var UseDepthNormalFromUser = reader.ReadBoolean();
    }
}