using AssetRipper.SourceGenerated.Classes.ClassID_74;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.HoukaiCommon;

public static class AclHook
{
    [RetargetMethodFunc(typeof(AnimationClip_2017_3_0))]
    private static bool AnimationClip_2017_3_0_ReadRelease(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
        ilCursor.GotoPrev();
        ilCursor.Emit(OpCodes.Ldarg_0);
        ilCursor.Emit(OpCodes.Ldc_I4_0);
        ilCursor.Emit(OpCodes.Stfld, typeof(AnimationClip_2017_3_0).GetField("m_Compressed", ReflectionExtensions.PrivateInstanceBindFlag()));
        return true;
    }
}