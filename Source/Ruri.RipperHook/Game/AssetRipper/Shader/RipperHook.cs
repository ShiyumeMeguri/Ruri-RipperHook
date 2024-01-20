using AssetRipper.Export.UnityProjects;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Ruri.RipperHook.ShaderDecompiler;
public static class RipperHook
{
	// DXDecompile Retarget
	[RetargetMethodFunc(typeof(Ripper), nameof(OverrideNormalExporters))]
	private static bool OverrideNormalExporters(ILContext il)
	{
		ILCursor ilCursor = new ILCursor(il);

		int newobjIndex = -1;
		if (ilCursor.TryGotoNext(instr => instr.OpCode == OpCodes.Newobj
										  && instr.Operand is MethodReference methodRef
										  && methodRef.DeclaringType.FullName == "AssetRipper.Export.UnityProjects.Shaders.USCShaderExporter"
										  && methodRef.Name == ".ctor"))
		{
			newobjIndex = ilCursor.Index;

			if (ilCursor.TryGotoPrev(instr => instr.OpCode == OpCodes.Switch))
			{
				ILLabel[] labels = (ILLabel[])ilCursor.Next.Operand;
				labels[2] = il.DefineLabel();
				ilCursor.Goto(newobjIndex);
				ilCursor.Remove();
				ilCursor.MarkLabel(labels[2]);
				ilCursor.Emit(OpCodes.Newobj, typeof(ShaderDXDecompileExporter).GetConstructor(Type.EmptyTypes));
				return true;
			}
		}
		return false;
	}
}
