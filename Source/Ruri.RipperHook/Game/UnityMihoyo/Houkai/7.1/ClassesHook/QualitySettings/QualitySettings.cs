using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.QualitySetting;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(QualitySetting_2017))]
    public void QualitySetting_2017_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as QualitySetting_2017;
        var type = typeof(QualitySetting_2017);

        _this.Name = reader.ReadRelease_Utf8StringAlign();
        _this.PixelLightCount = reader.ReadInt32();
        _this.Shadows = reader.ReadInt32();
        _this.ShadowResolution = reader.ReadInt32();
        _this.ShadowProjection = reader.ReadInt32();
        _this.ShadowCascades = reader.ReadInt32();
        _this.ShadowDistance = reader.ReadSingle();
        _this.ShadowNearPlaneOffset = reader.ReadSingle();
        _this.ShadowCascade2Split = reader.ReadSingle();
        _this.ShadowCascade4Split.ReadRelease(ref reader);
        _this.ShadowmaskMode = reader.ReadInt32();
        _this.BlendWeights = reader.ReadInt32();
        _this.TextureQuality = reader.ReadInt32();
        _this.AnisotropicTextures = reader.ReadInt32();
        _this.AntiAliasing = reader.ReadInt32();
        _this.SoftParticles = reader.ReadBoolean();
        _this.SoftVegetation = reader.ReadBoolean();
        _this.RealtimeReflectionProbes = reader.ReadBoolean();
        _this.BillboardsFaceCameraPosition = reader.ReadRelease_BooleanAlign();
        _this.VSyncCount = reader.ReadInt32();
        _this.LodBias = reader.ReadSingle();
        _this.MaximumLODLevel = reader.ReadInt32();
        var textureStreamingBudget = reader.ReadSingle();
        var minStreamingMipLevel = reader.ReadInt32();
        var textureStreamingEnabled = reader.ReadBoolean();
        var textureStreamingForceLoadEnabled = reader.ReadRelease_BooleanAlign();
        var forceLoadStreamingMipLevel = reader.ReadRelease_Int32Align();
        _this.ParticleRaycastBudget = reader.ReadInt32();
        _this.AsyncUploadTimeSlice = reader.ReadInt32();
        _this.AsyncUploadBufferSize = reader.ReadInt32();
        _this.ResolutionScalingFixedDPIFactor = reader.ReadRelease_SingleAlign();
        var meshDynamicCompressionLevel = reader.ReadRelease_Int32Align();
        var useOctagonParticleGlobal = reader.ReadRelease_BooleanAlign();
        var particleEmitLevel = reader.ReadRelease_Int32Align();
        var useParticleDistanceLOD = reader.ReadRelease_BooleanAlign();
        var particleLODDistance = reader.ReadRelease_SingleAlign();
        var useMeshParticleInstancing = reader.ReadBoolean();
        var useInstancingAcrossPS = reader.ReadRelease_BooleanAlign();
        var lightOnLevel = reader.ReadRelease_Int32Align();
        var perObjetShadowQuality = reader.ReadRelease_Int32Align();
        var optimizeUIUpdate = reader.ReadRelease_BooleanAlign();
        var avatarOutlineThresh = reader.ReadSingle();
        var avatarShadowThresh = reader.ReadSingle();
        var avatarMotionVectorThresh = reader.ReadSingle();
    }
}