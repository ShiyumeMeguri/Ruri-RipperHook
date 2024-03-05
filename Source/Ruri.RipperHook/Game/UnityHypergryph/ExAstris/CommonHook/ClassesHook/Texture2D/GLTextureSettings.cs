using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.GLTextureSettings;

namespace Ruri.RipperHook.ExAstrisCommon;

public partial class ExAstrisCommon_Hook
{
    [RetargetMethod(typeof(GLTextureSettings_2017))]
    public void GLTextureSettings_2017_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as GLTextureSettings_2017;
        var type = typeof(GLTextureSettings_2017);

        _this.FilterMode = reader.ReadInt32();
        _this.Aniso = reader.ReadInt32();
        _this.MipBias = reader.ReadSingle();
        var m_TextureGroup = reader.ReadInt32();
        _this.WrapU = reader.ReadInt32();
        _this.WrapV = reader.ReadInt32();
        _this.WrapW = reader.ReadInt32();
    }
}