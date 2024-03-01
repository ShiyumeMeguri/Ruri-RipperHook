using AssetRipper.IO.Files.BundleFiles.FileStream;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.UnityChinaCommon;

public partial class UnityChinaCommon_Hook
{
    [RetargetMethodFunc(typeof(FileStreamBundleFile), nameof(ReadFileStreamData))]
    private static bool ReadFileStreamData(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        ilCursor.TryGotoNext(instr => instr.OpCode == OpCodes.Call && instr.Operand is MethodReference methodRef && methodRef.Name == "GetBlockInfoNeedPaddingAtStart");
        if (ilCursor.TryGotoNext(instr => instr.OpCode == OpCodes.Brfalse_S))
        {
            var labels = (ILLabel)ilCursor.Next.Operand;
            ilCursor.Remove();
            ilCursor.Emit(OpCodes.Brtrue_S, labels);
            return true;
        }

        return false;
    }
}