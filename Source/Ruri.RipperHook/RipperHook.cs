using System.Reflection;
using AssetRipper.Assets;
using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using MonoMod.Cil;

namespace Ruri.RipperHook;

public abstract class RipperHook
{
    protected List<string> additionalNamespaces = new();
    protected List<string> excludedNamespaces = new();

    protected RipperHook()
    {
        InitAttributeHook();
    }

    protected virtual void AddExtraHook(string nameSpace, Action action)
    {
        additionalNamespaces.Add(nameSpace);
        action();
    }
    protected virtual void InitAttributeHook()
    {
        var bindingFlags = ReflectionExtensions.AnyBindFlag();
        var namespacesToConsider = new List<string> { GetType().Namespace };
        namespacesToConsider.AddRange(additionalNamespaces); // 添加一些通用空间 避免写编写重复代码
        namespacesToConsider = namespacesToConsider.Where(ns => !excludedNamespaces.Contains(ns)).ToList(); // 排除继承等情况导致重复的hook

        var assembly = this.GetType().Assembly; // 要用this获取真实类型
        var types = assembly.GetTypes();

        // 包括处理嵌套类
        var allTypes = types.Concat(types.SelectMany(t => t.GetNestedTypes(bindingFlags)));
        var methods = allTypes
            .Where(t => t.Namespace != null && namespacesToConsider.Any(ns => t.Namespace.StartsWith(ns)))
            .SelectMany(t => t.GetMethods(bindingFlags));

        // 方法转发处理
        var targetMethods = methods.Where(m => m.GetCustomAttributes<RetargetMethodAttribute>(true).Any());
        foreach (var methodDest in targetMethods)
        {
            var attrs = methodDest.GetCustomAttributes<RetargetMethodAttribute>().ToArray();
            foreach (var attr in attrs)
            {
                MethodInfo? methodSrc;
                if (attr.MethodParameters == null)
                {
                    methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags);
                }
                else
                {
                    methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags, attr.MethodParameters);
                }
                int srcParameterCount = methodSrc.GetParameters().Length;
                int destParameterCount = methodDest.GetParameters().Length;
                if (srcParameterCount != destParameterCount)
                {
                    throw new Exception("Hook函数和目标函数参数数量不一致 如果是静态方法 看括号内参数数量 如果是实例方法 需要+1 因为this始终在实例方法中传递");
                }
                if (methodSrc.IsStatic)
                    srcParameterCount--;

                ReflectionExtensions.RetargetCall(methodSrc, methodDest, srcParameterCount, attr.IsBefore,attr.IsReturn);
            }
        }
        // 字节码插入处理
        var targetFuncMethods = methods.Where(m => m.GetCustomAttributes<RetargetMethodFuncAttribute>(true).Any());
        foreach (var methodDest in targetFuncMethods)
        {
            var attrs = methodDest.GetCustomAttributes<RetargetMethodFuncAttribute>().ToArray();
            foreach (var attr in attrs)
            {
                MethodInfo? methodSrc;
                if (attr.MethodParameters == null)
                {
                    methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags);
                }
                else
                {
                    methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags, attr.MethodParameters);
                }

                var HookCallback = (Func<ILContext, bool>)Delegate.CreateDelegate(typeof(Func<ILContext, bool>), methodDest);
                ReflectionExtensions.RetargetCallFunc(HookCallback, methodSrc);
            }
        }
        // 字节码插入处理 构造函数版
        var targetCtorFuncMethods = methods.Where(m => m.GetCustomAttributes<RetargetMethodCtorFuncAttribute>(true).Any());
        foreach (var methodDest in targetCtorFuncMethods)
        {
            var attrs = methodDest.GetCustomAttributes<RetargetMethodCtorFuncAttribute>().ToArray();
            foreach (var attr in attrs)
            {
                ConstructorInfo? methodSrc;
                if (attr.MethodParameters == null)
                {
                    methodSrc = attr.SourceType.GetConstructor(Type.EmptyTypes);
                }
                else
                {
                    methodSrc = attr.SourceType.GetConstructor(bindingFlags, attr.MethodParameters);
                }

                var HookCallback = (Func<ILContext, bool>)Delegate.CreateDelegate(typeof(Func<ILContext, bool>), methodDest);
                ReflectionExtensions.RetargetCallCtorFunc(HookCallback, methodSrc);
            }
        }
    }

    protected void SetPrivateField(Type type, string name, object newValue)
    {
        type.GetField(name, ReflectionExtensions.PrivateInstanceBindFlag()).SetValue(this, newValue);
    }

    protected object GetPrivateField(Type type, string name)
    {
        return type.GetField(name, ReflectionExtensions.PrivateInstanceBindFlag()).GetValue(this);
    }

    protected void SetAssetListField<T>(Type type, string name, ref EndianSpanReader reader, bool isAlign = true) where T : UnityAssetBase, new()
    {
        var field = type.GetField(name, ReflectionExtensions.PrivateInstanceBindFlag());

        var fieldType = field.FieldType;
        var filedObj = Activator.CreateInstance(fieldType);
        if (isAlign)
            ((AssetList<T>)filedObj).ReadRelease_ArrayAlign_Asset(ref reader);
        else
            ((AssetList<T>)filedObj).ReadRelease_Array_Asset(ref reader);

        field.SetValue(this, filedObj);
    }
}