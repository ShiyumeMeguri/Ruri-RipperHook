using AssetRipper.Import.Configuration;
using AssetRipper.Primitives;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.AR_BundledAssetsExportMode;

public partial class AR_BundledAssetsExportMode_Hook
{
    [RetargetMethodCtorFunc(typeof(ImportSettings))]
    public static bool Ctor(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
        ilCursor.GotoPrev();
        ilCursor.Emit(OpCodes.Ldarg_0);
        ilCursor.Emit(OpCodes.Ldc_I4_2);
        var destMethod = typeof(ImportSettings).GetMethod("set_BundledAssetsExportMode");
        ilCursor.Emit(OpCodes.Call, destMethod);
        return true;
    }
}