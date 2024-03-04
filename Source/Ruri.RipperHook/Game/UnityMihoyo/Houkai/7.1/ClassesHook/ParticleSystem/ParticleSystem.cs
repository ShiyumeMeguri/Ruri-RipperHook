using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_198;
using AssetRipper.SourceGenerated.Subclasses.MinMaxCurve;
using Ruri.RipperHook.Houkai_7_1.Classes.Subclasses.TextModule;

namespace Ruri.RipperHook.Houkai_7_1;

public partial class Houkai_7_1_Hook
{
    [RetargetMethod(typeof(ParticleSystem_2017_3))]
    public void ParticleSystem_2017_3_ReadRelease(ref EndianSpanReader reader)
    {
        var _this = (object)this as ParticleSystem_2017_3;
        var type = typeof(ParticleSystem_2017_3);

        _this.GameObject.ReadRelease(ref reader);
        _this.LengthInSec = reader.ReadSingle();
        _this.SimulationSpeed = reader.ReadSingle();
        _this.StopAction = reader.ReadInt32();
        _this.CullingMode = reader.ReadInt32();
        _this.Looping = reader.ReadBoolean();
        _this.Prewarm = reader.ReadBoolean();
        _this.PlayOnAwake = reader.ReadBoolean();
        _this.UseUnscaledTime = reader.ReadBoolean();
        _this.AutoRandomSeed = reader.ReadBoolean();
        _this.UseRigidbodyForVelocity = reader.ReadRelease_BooleanAlign();
        ((MinMaxCurve_2017_1_0_b3)_this.StartDelay_MinMaxCurve).ReadRelease_AssetAlign(ref reader);
        _this.MoveWithTransform_Int32 = reader.ReadRelease_Int32Align();
        _this.MoveWithCustomTransform.ReadRelease(ref reader);
        _this.ScalingMode = reader.ReadInt32();
        _this.RandomSeed_Int32 = reader.ReadInt32();
        _this.InitialModule.ReadRelease(ref reader);
        _this.ShapeModule.ReadRelease(ref reader);
        _this.EmissionModule.ReadRelease(ref reader);
        _this.SizeModule.ReadRelease(ref reader);
        _this.RotationModule.ReadRelease(ref reader);
        _this.ColorModule.ReadRelease(ref reader);
        _this.UVModule.ReadRelease(ref reader);
        _this.VelocityModule.ReadRelease(ref reader);
        _this.InheritVelocityModule.ReadRelease(ref reader);
        _this.ForceModule.ReadRelease(ref reader);
        _this.ExternalForcesModule.ReadRelease(ref reader);
        _this.ClampVelocityModule.ReadRelease(ref reader);
        _this.NoiseModule.ReadRelease(ref reader);
        _this.SizeBySpeedModule.ReadRelease(ref reader);
        _this.RotationBySpeedModule.ReadRelease(ref reader);
        _this.ColorBySpeedModule.ReadRelease(ref reader);
        _this.CollisionModule.ReadRelease(ref reader);
        _this.TriggerModule.ReadRelease(ref reader);
        _this.SubModule.ReadRelease(ref reader);
        _this.LightsModule.ReadRelease(ref reader);
        _this.TrailModule.ReadRelease(ref reader);
        _this.CustomDataModule.ReadRelease(ref reader);
        TextModule textModule = new();
        textModule.ReadRelease(ref reader);
    }
}