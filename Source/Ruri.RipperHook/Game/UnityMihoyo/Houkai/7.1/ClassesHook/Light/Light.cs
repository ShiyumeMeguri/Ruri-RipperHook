using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_108;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(Light_2017_3_0))]
    public void Light_2017_3_0_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Light_2017_3_0;
        var type = typeof(Light_2017_3_0);

        _this.GameObject.ReadRelease(ref reader);
        _this.Enabled = reader.ReadRelease_ByteAlign();
        _this.Type = reader.ReadInt32();
        _this.Color.ReadRelease(ref reader);
        _this.Intensity = reader.ReadSingle();
        _this.Range = reader.ReadSingle();
        _this.SpotAngle = reader.ReadSingle();
        _this.CookieSize = reader.ReadSingle();
        _this.Shadows.ReadRelease(ref reader);
        _this.Cookie.ReadRelease(ref reader);
        _this.DrawHalo = reader.ReadRelease_BooleanAlign();
        _this.BakingOutput.ReadRelease(ref reader);
        _this.Flare.ReadRelease(ref reader);
        _this.RenderMode = reader.ReadInt32();
        _this.CullingMask.ReadRelease(ref reader);
        _this.Lightmapping = reader.ReadInt32();
        _this.AreaSize.ReadRelease(ref reader);
        _this.BounceIntensity = reader.ReadSingle();
        _this.ColorTemperature = reader.ReadSingle();
        _this.UseColorTemperature = reader.ReadRelease_BooleanAlign();
        var m_LightOnLevel = reader.ReadInt32();
    }
}