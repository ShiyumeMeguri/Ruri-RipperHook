using AssetRipper.Export.UnityProjects.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace Ruri.RipperHook.ShaderDecompiler;
public static class LibraryConfigurationHook
{
	// Set DXDecompile Default Value
	[RetargetMethodFunc(typeof(LibraryConfiguration), nameof(ResetToDefaultValues))]
	private static bool ResetToDefaultValues(ILContext il)
	{
		ILCursor ilCursor = new ILCursor(il);
		while (ilCursor.TryGotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ret)) ;
		ilCursor.GotoPrev();
		ilCursor.Emit(OpCodes.Ldarg_0);
		ilCursor.Emit(OpCodes.Ldc_I4_3);
		MethodInfo destMethod = typeof(LibraryConfiguration).GetMethod("set_ShaderExportMode");
		ilCursor.Emit(OpCodes.Call, destMethod);
		return true;
	}
}
