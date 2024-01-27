using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.EmissionModule;
using AssetRipper.SourceGenerated.Subclasses.ParticleSystemEmissionBurst;
using AssetRipper.SourceGenerated.Subclasses.Vector4f;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(EmissionModule_2017_3_0))]
    public void EmissionModule_2017_3_0_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as EmissionModule_2017_3_0;
        var type = typeof(EmissionModule_2017_3_0);

        _this.Enabled = reader.ReadRelease_BooleanAlign();
        _this.RateOverTime.ReadRelease(ref reader);
        _this.RateOverDistance.ReadRelease(ref reader);
        _this.BurstCount_Int32 = reader.ReadRelease_Int32Align();
        SetAssetListField<ParticleSystemEmissionBurst_2017_3_0>(type, "m_Bursts", ref reader);
        Vector4f m_EmissionLevel = new();
        m_EmissionLevel.ReadRelease(ref reader);
        var m_EmissionFalloffStart = reader.ReadSingle();
        var m_EmissionFalloffEnd = reader.ReadSingle();
        var m_EnableFallOff = reader.ReadRelease_BooleanAlign();
    }
}