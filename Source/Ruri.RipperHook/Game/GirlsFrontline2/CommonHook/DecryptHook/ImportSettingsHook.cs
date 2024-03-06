using AssetRipper.Import.Configuration;
using AssetRipper.Primitives;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.GirlsFrontline2Common;

public partial class GirlsFrontline2Common_Hook
{
    [RetargetMethodCtorFunc(typeof(ImportSettings))]
    public static bool Ctor(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
        ilCursor.GotoPrev();
        ilCursor.Emit(OpCodes.Ldarg_0);
        ilCursor.Emit(OpCodes.Ldc_I4, 2019);
        ilCursor.Emit(OpCodes.Ldc_I4, 4);
        ilCursor.Emit(OpCodes.Ldc_I4, 40);
        ilCursor.Emit(OpCodes.Ldc_I4, 3);
        ilCursor.Emit(OpCodes.Ldc_I4, 1);
        ilCursor.Emit(OpCodes.Newobj, il.Import(typeof(UnityVersion).GetConstructor(new Type[] { typeof(ushort), typeof(ushort), typeof(ushort), typeof(UnityVersionType), typeof(byte) })));
        var destMethod = typeof(ImportSettings).GetMethod("set_DefaultVersion");
        ilCursor.Emit(OpCodes.Call, destMethod);
        return true;
    }
}