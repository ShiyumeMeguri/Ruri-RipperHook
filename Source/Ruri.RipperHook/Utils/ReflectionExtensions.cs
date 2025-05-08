using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using System.Collections.Concurrent;
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

    public static void RetargetCallCtorFunc(Func<ILContext, bool> func, ConstructorInfo srcMethod)
    {
        var hookDest = new ILContext.Manipulator(il =>
        {
            if (!func(il))
                throw new Exception($"Hook {srcMethod.DeclaringType.Name}.{srcMethod.Name} Fail");
        });
        RuriRuntimeHook.ilHooks.Add(new ILHook(srcMethod, hookDest));
        Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
    }

    /// <summary>
    /// 默认的情况下是从起点插入并直接返回(替换原函数)
    /// isBefore可以选择从前后插入代码
    /// isReturn可以选择不返回 也就是继续执行原函数之后的代码
    /// </summary>
    /// <param name="srcMethod"></param>
    /// <param name="targetMethod"></param>
    /// <param name="maxArgIndex"></param>
    /// <param name="isBefore"></param>
    /// <param name="isReturn"></param>
    public static void RetargetCall(MethodInfo srcMethod, MethodInfo targetMethod, int maxArgIndex = 1, bool isBefore = true, bool isReturn = true)
    {
        var hookDest = new ILContext.Manipulator(il =>
        {
            var ilCursor = new ILCursor(il);
            Action inject = () =>
            {
                for (var i = 0; i <= maxArgIndex; i++)
                {
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
                }
                ilCursor.Emit(OpCodes.Call, targetMethod);
                if (isReturn)
                {
                    ilCursor.Emit(OpCodes.Ret);
                }
                ilCursor.SearchTarget = SearchTarget.Next; // 保证插入后从当前位置继续查找避免死循环
            };

            if (!isBefore) // 从起点注入还是末尾注入
                while (ilCursor.TryGotoNext(MoveType.Before, instr => instr.OpCode == OpCodes.Ret))
                    inject();
            else
                inject();
        });

        RuriRuntimeHook.ilHooks.Add(new ILHook(srcMethod, hookDest));
        Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
    }

    #endregion

    private static readonly ConcurrentDictionary<string, Action<object, object>> _cache = new();

    /// <summary>
    /// 将 src 实例中所有同名且类型兼容的字段值拷贝到 dst 实例。
    /// </summary>
    public static void ClassCopy(object src, object dst)
    {
        if (src == null) throw new ArgumentNullException(nameof(src));
        if (dst == null) throw new ArgumentNullException(nameof(dst));

        var key = src.GetType().FullName + "->" + dst.GetType().FullName;
        var copier = _cache.GetOrAdd(key, _ => CreateCopier(src.GetType(), dst.GetType()));
        copier(src, dst);
    }

    private static Action<object, object> CreateCopier(Type srcType, Type dstType)
    {
        const BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var srcFields = srcType.GetFields(flag);
        var dstFields = dstType.GetFields(flag);
        var dstMap = new ConcurrentDictionary<string, FieldInfo>();
        foreach (var df in dstFields)
            dstMap[df.Name] = df;

        // 创建 DynamicMethodDefinition: void Copy(object src, object dst)
        var dmd = new DynamicMethodDefinition(
            $"Copy_{srcType.Name}_To_{dstType.Name}",
            typeof(void),
            new Type[] { typeof(object), typeof(object) }
        );
        var methodDef = dmd.Definition;
        var il = methodDef.Body.GetILProcessor();
        var module = methodDef.Module;

        // 声明局部变量：srcType loc0, dstType loc1
        var locSrc = new VariableDefinition(module.ImportReference(srcType));
        var locDst = new VariableDefinition(module.ImportReference(dstType));
        methodDef.Body.Variables.Add(locSrc);
        methodDef.Body.Variables.Add(locDst);
        methodDef.Body.InitLocals = true;

        // loc0 = (srcType) arg0
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Castclass, module.ImportReference(srcType));
        il.Emit(OpCodes.Stloc, locSrc);
        // loc1 = (dstType) arg1
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Castclass, module.ImportReference(dstType));
        il.Emit(OpCodes.Stloc, locDst);

        // 同名字段拷贝
        foreach (var sf in srcFields)
        {
            if (dstMap.TryGetValue(sf.Name, out var df) && df.FieldType.GetManagedSize() == sf.FieldType.GetManagedSize())
            {
                il.Emit(OpCodes.Ldloc, locDst);
                il.Emit(OpCodes.Ldloc, locSrc);
                il.Emit(OpCodes.Ldfld, module.ImportReference(sf));
                il.Emit(OpCodes.Stfld, module.ImportReference(df));
            }
            else
            {
                throw new InvalidOperationException($"Cannot write field '{sf.Name}' from {srcType.FullName} to {dstType.FullName}");
            }
        }

        il.Emit(OpCodes.Ret);
        var generated = dmd.Generate();
        return generated.CreateDelegate<Action<object, object>>();
    }
}
