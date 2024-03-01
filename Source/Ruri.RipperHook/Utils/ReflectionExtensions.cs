using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ruri.RipperHook;

public static class ReflectionExtensions
{
    #region 通用判断

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BindingFlags AnyBindFlag()
    {
        return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BindingFlags PublicInstanceBindFlag()
    {
        return BindingFlags.Public | BindingFlags.Instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BindingFlags PrivateInstanceBindFlag()
    {
        return BindingFlags.NonPublic | BindingFlags.Instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BindingFlags PublicStaticBindFlag()
    {
        return BindingFlags.Public | BindingFlags.Static;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BindingFlags PrivateStaticBindFlag()
    {
        return BindingFlags.NonPublic | BindingFlags.Static;
    }

    #endregion

    #region 方法反射

    public static void RetargetCallFunc(Func<ILContext, bool> func, MethodInfo srcMethod)
    {
        var hookDest = new ILContext.Manipulator(il =>
        {
            if (!func(il))
                throw new Exception($"Hook {srcMethod.DeclaringType.Name}.{srcMethod.Name} Fail");
        });
        RuriRuntimeHook.ilHooks.Add(new ILHook(srcMethod, hookDest));
        Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
    }

    public static void RetargetCall(MethodInfo srcMethod, MethodInfo targetMethod, int argsCount = 1, bool isBefore = true, bool isReturn = true)
    {
        var hookDest = new ILContext.Manipulator(il =>
        {
            var ilCursor = new ILCursor(il);
            if (!isBefore) // 从起点注入还是末尾注入
                while (ilCursor.TryGotoNext(MoveType.Before, instr => instr.OpCode == OpCodes.Ret))
                    ;
            for (var i = 0; i <= argsCount; i++)
                switch (i)
                {
                    case 0:
                        ilCursor.Emit(OpCodes.Ldarg_0);
                        continue;
                    case 1:
                        ilCursor.Emit(OpCodes.Ldarg_1);
                        continue;
                    case 2:
                        ilCursor.Emit(OpCodes.Ldarg_2);
                        continue;
                    case 3:
                        ilCursor.Emit(OpCodes.Ldarg_3);
                        continue;
                    default:
                        ilCursor.Emit(OpCodes.Ldarg, i);
                        continue;
                }

            ilCursor.Emit(OpCodes.Call, targetMethod);
            if (isReturn)
                ilCursor.Emit(OpCodes.Ret);
        });

        RuriRuntimeHook.ilHooks.Add(new ILHook(srcMethod, hookDest));
        Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
    }

    #endregion
}