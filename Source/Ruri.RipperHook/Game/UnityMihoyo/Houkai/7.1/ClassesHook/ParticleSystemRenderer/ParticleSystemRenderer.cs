using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_199;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(ParticleSystemRenderer_2017_3))]
    public void ParticleSystemRenderer_2017_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as ParticleSystemRenderer_2017_3;
        var type = typeof(ParticleSystemRenderer_2017_3);

        Renderer_2017_3_ReadRelease(ref reader);
        _this.RenderMode_UInt16 = reader.ReadUInt16();
        _this.SortMode_UInt16 = reader.ReadUInt16();
        _this.MinParticleSize = reader.ReadSingle();
        _this.MaxParticleSize = reader.ReadSingle();
        _this.CameraVelocityScale = reader.ReadSingle();
        _this.VelocityScale = reader.ReadSingle();
        _this.LengthScale = reader.ReadSingle();
        _this.SortingFudge = reader.ReadSingle();
        _this.NormalDirection = reader.ReadSingle();
        _this.RenderAlignment = reader.ReadInt32();
        _this.Pivot.ReadRelease(ref reader);
        _this.UseCustomVertexStreams = reader.ReadBoolean();
        var m_EnableGPUInstancing = reader.ReadRelease_BooleanAlign();
        _this.VertexStreams = reader.ReadRelease_ArrayAlign_Byte();
        _this.Mesh.ReadRelease(ref reader);
        _this.Mesh1.ReadRelease(ref reader);
        _this.Mesh2.ReadRelease(ref reader);
        _this.Mesh3.ReadRelease(ref reader);
        _this.MaskInteraction = reader.ReadInt32();
    }
}