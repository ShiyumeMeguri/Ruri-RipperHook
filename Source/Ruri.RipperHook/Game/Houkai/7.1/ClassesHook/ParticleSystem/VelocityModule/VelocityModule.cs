using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Subclasses.MinMaxCurve;
using AssetRipper.SourceGenerated.Subclasses.VelocityModule;

namespace Ruri.RipperHook.Houkai_7_1;
public partial class Houkai_7_1_Hook
{
	[RetargetMethod(typeof(VelocityModule_2017_3_0))]
	public void VelocityModule_2017_3_0_ReadRelease(ref EndianSpanReader reader)
	{
		var _this = (object)this as VelocityModule_2017_3_0;
		var type = typeof(VelocityModule_2017_3_0);

		MinMaxCurve_2017_1_0_b3 OrbitalX = new();
		MinMaxCurve_2017_1_0_b3 OrbitalY = new();
		MinMaxCurve_2017_1_0_b3 OrbitalZ = new();
		MinMaxCurve_2017_1_0_b3 OrbitalOffsetX = new();
		MinMaxCurve_2017_1_0_b3 OrbitalOffsetY = new();
		MinMaxCurve_2017_1_0_b3 OrbitalOffsetZ = new();
		MinMaxCurve_2017_1_0_b3 Radial = new();

		_this.Enabled = reader.ReadRelease_BooleanAlign();
		_this.X.ReadRelease(ref reader);
		_this.Y.ReadRelease(ref reader);
		_this.Z.ReadRelease(ref reader);
		OrbitalX.ReadRelease(ref reader);
		OrbitalY.ReadRelease(ref reader);
		OrbitalZ.ReadRelease(ref reader);
		OrbitalOffsetX.ReadRelease(ref reader);
		OrbitalOffsetY.ReadRelease(ref reader);
		OrbitalOffsetZ.ReadRelease(ref reader);
		Radial.ReadRelease(ref reader);
		_this.SpeedModifier.ReadRelease(ref reader);
		_this.InWorldSpace = reader.ReadRelease_BooleanAlign();
	}
}
