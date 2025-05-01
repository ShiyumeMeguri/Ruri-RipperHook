using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_95;

namespace Ruri.RipperHook.StarRail_3_2;

public partial class StarRail_3_2_Hook
{
    [RetargetMethod(typeof(Animator_2018_3))]
    public void Animator_2018_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as Animator_2018_3;
        var type = typeof(Animator_2018_3);

        _this.GameObject.ReadRelease(ref reader);
        _this.Enabled =  reader.ReadRelease_ByteAlign();
        _this.Avatar.ReadRelease(ref reader);
        _this.Controller_PPtr_RuntimeAnimatorController_5.ReadRelease(ref reader);
        _this.CullingMode = reader.ReadInt32();
        _this.UpdateMode = reader.ReadInt32();
        var m_MotionSkeletonMode = reader.ReadInt32();
        _this.ApplyRootMotion = reader.ReadBoolean();
        _this.LinearVelocityBlending =  reader.ReadRelease_BooleanAlign();
        _this.HasTransformHierarchy = reader.ReadBoolean();
        _this.AllowConstantClipSamplingOptimization = reader.ReadBoolean();
        _this.KeepAnimatorControllerStateOnDisable =  reader.ReadRelease_BooleanAlign();
    }
}