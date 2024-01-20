using AssetRipper.IO.Endian;
using AssetRipper.SourceGenerated.Classes.ClassID_319;

namespace Ruri.RipperHook.Houkai_7_1;
public partial class Houkai_7_1_Hook
{
	[RetargetMethod(typeof(AvatarMask_2017_3_0))]
	public void AvatarMask_2017_3_0_ReadRelease(ref EndianSpanReader reader)
	{
		var _this = (object)this as AvatarMask_2017_3_0;
		var type = typeof(AvatarMask_2017_3_0);

		_this.Name = reader.ReadRelease_Utf8StringAlign();
		_this.Mask.ReadRelease_ArrayAlign_UInt32(ref reader);
		_this.Elements.ReadRelease_ArrayAlign_Asset(ref reader);
		bool m_UnlockEditForCopyFromOtherMask = reader.ReadRelease_BooleanAlign();
		bool m_UnlockEditorForHumanoid = reader.ReadRelease_BooleanAlign();
	}
}
