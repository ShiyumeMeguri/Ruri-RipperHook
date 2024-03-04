using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_111;
using AssetRipper.SourceGenerated.Subclasses.PPtr_AnimationClip;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(Animation_2017_3))]
    public void Animation_2017_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Animation_2017_3;
        var type = typeof(Animation_2017_3);

        _this.GameObject.ReadRelease(ref reader);
        _this.Enabled = reader.ReadRelease_ByteAlign();
        _this.Animation.ReadRelease(ref reader);
        SetAssetListField<PPtr_AnimationClip_5>(type, "m_Animations", ref reader);
        _this.WrapMode = reader.ReadInt32();
        _this.PlayAutomatically = reader.ReadBoolean();
        _this.AnimatePhysics = reader.ReadRelease_BooleanAlign();
        var m_DeactivateMode = reader.ReadInt32();
        _this.CullingType = reader.ReadInt32();
    }
}