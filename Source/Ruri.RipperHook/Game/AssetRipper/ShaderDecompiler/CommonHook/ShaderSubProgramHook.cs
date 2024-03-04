using AssetRipper.Export.Modules.Shaders.ShaderBlob;
using AssetRipper.IO.Endian;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.AR_ShaderDecompiler;

public partial class AR_ShaderDecompiler_Hook
{
    // Fix 2019.4 DXDecompile Data
    [RetargetMethodFunc(typeof(ShaderSubProgram), nameof(Read))]
    private static bool Read(ILContext il)
    {
        var ilCursor = new ILCursor(il);
        while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
        ilCursor.GotoPrev();
        ilCursor.Emit(OpCodes.Ldarg_0);
        ilCursor.Emit(OpCodes.Ldarg_0);
        var getProgramDataMethod = typeof(ShaderSubProgram).GetMethod("get_ProgramData");
        ilCursor.Emit(OpCodes.Call, getProgramDataMethod);
        var destMethod = typeof(AR_ShaderDecompiler_Hook).GetMethod(nameof(ProcessShaderData), ReflectionExtensions.PublicStaticBindFlag());
        ilCursor.Emit(OpCodes.Call, destMethod);
        var setProgramDataMethod = typeof(ShaderSubProgram).GetMethod("set_ProgramData");
        ilCursor.Emit(OpCodes.Call, setProgramDataMethod);
        return true;
    }

    public static byte[] ProcessShaderData(byte[] programData)
    {
        //!!2019.4 support Hard Code!!
        if (programData.Length > 0 && programData[0] >= 2)
        {
            var reader = new EndianSpanReader(programData, EndianType.LittleEndian);
            var newData = new List<byte>(programData.Length - 0x20);
            newData.AddRange(reader.ReadBytes(6));
            reader.Position += 0x20;
            newData.AddRange(reader.ReadBytes(programData.Length - 0x26));
            programData = newData.ToArray();
        }

        return programData;
    }
}