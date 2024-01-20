using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_30;
using AssetRipper.SourceGenerated.Subclasses.BuiltinShaderSettings;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Shader;

namespace Ruri.RipperHook.Houkai_3_8;
public partial class Houkai_3_8_Hook
{
	[RetargetMethod(typeof(GraphicsSettings_2017_3_0))]
	public void GraphicsSettings_2017_3_0_ReadRelease(ref EndianSpanReader reader)
	{
		var _this = (object)this as GraphicsSettings_2017_3_0;
		var type = typeof(GraphicsSettings_2017_3_0);

		_this.Deferred.ReadRelease(ref reader);
		_this.DeferredReflections.ReadRelease(ref reader);
		_this.ScreenSpaceShadows.ReadRelease(ref reader);
		_this.LegacyDeferred.ReadRelease(ref reader);
		_this.DepthNormals.ReadRelease(ref reader);
		_this.MotionVectors.ReadRelease(ref reader);
		_this.LightHalo.ReadRelease(ref reader);
		_this.LensFlare.ReadRelease(ref reader);
		BuiltinShaderSettings depthDownSample = new();
		depthDownSample.ReadRelease(ref reader);
		SetAssetListField<PPtr_Shader_5_0_0>(type, "m_AlwaysIncludedShaders", ref reader);
		_this.PreloadedShaders.ReadRelease_ArrayAlign_Asset(ref reader);
		_this.SpritesDefaultMaterial.ReadRelease(ref reader);
		_this.CustomRenderPipeline.ReadRelease(ref reader);
		_this.TransparencySortMode = reader.ReadInt32();
		_this.TransparencySortAxis.ReadRelease(ref reader);
		_this.TierSettings_Tier1.ReadRelease(ref reader);
		_this.TierSettings_Tier2.ReadRelease(ref reader);
		_this.TierSettings_Tier3.ReadRelease(ref reader);
		_this.ShaderDefinesPerShaderCompiler.ReadRelease_ArrayAlign_Asset(ref reader);
		_this.LightsUseLinearIntensity = reader.ReadBoolean();
		_this.LightsUseColorTemperature = reader.ReadBoolean();
	}
}
