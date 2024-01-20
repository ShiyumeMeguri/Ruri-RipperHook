using AssetRipper.Export.Modules.Shaders.ShaderBlob;
using AssetRipper.IO.Endian;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace Ruri.RipperHook.ShaderDecompiler;
public static class ShaderSubProgramHook
{
	// Fix 2019.4 DXDecompile Data
	[RetargetMethodFunc(typeof(ShaderSubProgram), nameof(Read))]
	private static bool Read(ILContext il)
	{
		ILCursor ilCursor = new ILCursor(il);
		while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
		ilCursor.GotoPrev();
		ilCursor.Emit(OpCodes.Ldarg_0);
		ilCursor.Emit(OpCodes.Ldarg_0);
		MethodInfo getProgramDataMethod = typeof(ShaderSubProgram).GetMethod("get_ProgramData");
		ilCursor.Emit(OpCodes.Call, getProgramDataMethod);
		MethodInfo destMethod = typeof(ShaderSubProgramHook).GetMethod(nameof(ProcessShaderData), ReflectionExtension.PublicStaticBindFlag());
		ilCursor.Emit(OpCodes.Call, destMethod);
		MethodInfo setProgramDataMethod = typeof(ShaderSubProgram).GetMethod("set_ProgramData");
		ilCursor.Emit(OpCodes.Call, setProgramDataMethod);
		return true;
	}
	public static byte[] ProcessShaderData(byte[] programData)
	{
		//!!2019.4 support Hard Code!!
		if (programData.Length > 0 && programData[0] >= 2)
		{
			var reader = new EndianSpanReader(programData, EndianType.LittleEndian);
			List<byte> newData = new List<byte>(programData.Length - 0x20);
			newData.AddRange(reader.ReadBytes(6));
			reader.Position += 0x20;
			newData.AddRange(reader.ReadBytes(programData.Length - 0x26));
			programData = newData.ToArray();
		}
		return programData;
	}
}
