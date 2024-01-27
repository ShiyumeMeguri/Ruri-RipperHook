using System.Runtime.InteropServices;
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
        ilCursor.Emit(OpCodes.Stfld,
            typeof(AnimationClip_2017_3_0).GetField("m_Compressed", ReflectionExtension.PrivateInstanceBindFlag()));
        return true;
    }

    public static void DecompressAll(byte[] data, out float[] values, out float[] times)
    {
        var decompressedClip = new DecompressedClip();
        DecompressAll(data, ref decompressedClip);

        values = new float[decompressedClip.ValuesCount];
        Marshal.Copy(decompressedClip.Values, values, 0, decompressedClip.ValuesCount);

        times = new float[decompressedClip.TimesCount];
        Marshal.Copy(decompressedClip.Times, times, 0, decompressedClip.TimesCount);

        Dispose(ref decompressedClip);
    }

    [DllImport("acl", CallingConvention = CallingConvention.Cdecl)]
    private static extern void DecompressAll(byte[] data, ref DecompressedClip decompressedClip);

    [DllImport("acl", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Dispose(ref DecompressedClip decompressedClip);

    public struct DecompressedClip
    {
        public IntPtr Values;
        public int ValuesCount;
        public IntPtr Times;
        public int TimesCount;
    }
}