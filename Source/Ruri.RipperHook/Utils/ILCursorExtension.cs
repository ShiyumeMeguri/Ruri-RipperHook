using AssetRipper.IO.Endian;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ruri.RipperHook;
public static class ILCursorExtension
{
	// 直接插入代码 但是否出错完全看编译器心情 所以不用了
	public static bool TypeTreeInject(this ILCursor ilCursor, MethodInfo destMethod, string injectAfterField, bool isRemoveAlign = false)
	{
		if (ilCursor.TryGotoNext(MoveType.After, instr => IsFieldReference(instr) && ((FieldReference)instr.Operand).Name == injectAfterField))
		{
			if (isRemoveAlign && ilCursor.TryGotoPrev(instr => instr.OpCode == OpCodes.Call || instr.OpCode == OpCodes.Callvirt)) // 少部分情况需要移除原有的对齐
			{
				MethodReference calledMethod = ilCursor.Instrs[ilCursor.Index].Operand as MethodReference;
				string newMethodName = calledMethod.Name.Replace("Align", "");
				MethodInfo newMethod = typeof(ReadReleaseMethods).GetMethod(newMethodName, ReflectionExtension.PublicStaticBindFlag());
				if (newMethod == null)
					newMethod = typeof(EndianSpanReader).GetMethod(newMethodName, ReflectionExtension.PublicStaticBindFlag());
				MethodReference newMethodRef = ilCursor.Module.ImportReference(newMethod);
				ilCursor.Instrs[ilCursor.Index].Operand = newMethodRef;
			}
			if (ilCursor.TryGotoPrev(MoveType.Before, instr => instr.OpCode == OpCodes.Ldarg_0))
			{
				ilCursor.Goto(ilCursor.Index + 4);
				ilCursor.Emit(OpCodes.Ldarg_0);
				ilCursor.Emit(OpCodes.Ldarg_1);
				ilCursor.Emit(OpCodes.Call, destMethod);
			}
			return true;
		}
		return false;
	}
	public static bool TypeTreeRemove(this ILCursor ilCursor, string injectAfterField)
	{
		if (ilCursor.TryGotoNext(MoveType.After, instr => IsFieldReference(instr) && ((FieldReference)instr.Operand).Name == injectAfterField))
		{
			if (ilCursor.TryGotoPrev(instr => instr.OpCode == OpCodes.Ldarg_0))
			{
				ilCursor.RemoveRange(4);
				return true;
			}
		}
		return false;
	}
	#region 通用判断
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsFieldReference(Instruction instr)
	{
		return instr.OpCode == OpCodes.Ldfld || instr.OpCode == OpCodes.Stfld || instr.OpCode == OpCodes.Ldsfld || instr.OpCode == OpCodes.Stsfld;
	}
	#endregion
}
