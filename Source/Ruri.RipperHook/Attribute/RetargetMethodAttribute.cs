using AssetRipper.Assets.Collections;
using AssetRipper.Assets.Metadata;
using AssetRipper.Primitives;
using AssetRipper.SourceGenerated;
using System.Diagnostics;
using System.Reflection;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodAttribute : Attribute
{
    public RetargetMethodAttribute(Type sourceType, string sourceMethodName = "ReadRelease", bool isBefore = true, bool isReturn = true, Type[] methodParameters = null)
    {
        SourceType = sourceType;
        SourceMethodName = sourceMethodName;
        IsBefore = isBefore;
        IsReturn = isReturn;
        MethodParameters = methodParameters;
    }

    public RetargetMethodAttribute(string sourceTypeName, string sourceMethodName = "ReadRelease", bool isBefore = true, bool isReturn = true, Type[] methodParameters = null)
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        SourceMethodName = sourceMethodName;
        IsBefore = isBefore;
        IsReturn = isReturn;
        MethodParameters = methodParameters;
    }

    /// <summary>
    /// 自动根据 ClassIDType + Unity 版本字符串，动态算出真正的目标类型。
    /// 用法示例：
    /// [RetargetMethod(ClassIDType.Camera, "2019.4.34f1")]
    /// </summary>
    public RetargetMethodAttribute(ClassIDType classIdType, string unityVersion, string sourceMethodName = "ReadRelease", bool isBefore = true, bool isReturn = true, Type[] methodParameters = null) : this(
            $"{GetSourceTypeFullName(classIdType, UnityVersion.Parse(unityVersion))}, {typeof(ClassIDType).Assembly.GetName().Name}",
            sourceMethodName,
            isBefore,
            isReturn,
            methodParameters)
    {
    }

    /// <summary>
    /// 通过反射调用工厂的 Create 方法，拿到实例后取其实际类型全名。
    /// </summary>
    public static string GetSourceTypeFullName(ClassIDType classIdType, UnityVersion version)
    {
        string factoryTypeName = $"AssetRipper.SourceGenerated.Classes.ClassID_{(int)classIdType}.{classIdType}";
        Assembly asm = typeof(ClassIDType).Assembly;
        Type factoryType = asm.GetType(factoryTypeName)
            ?? throw new InvalidOperationException($"找不到工厂类型: {factoryTypeName} in {asm.GetName().Name}");
        var mi = factoryType.GetMethod("Create", new[] { typeof(AssetInfo), typeof(UnityVersion) });
        if (mi == null)
            throw new InvalidOperationException($"在 {factoryTypeName} 中找不到 Create(AssetInfo,UnityVersion)");
        object instance = mi.Invoke(null, new object[] { null, version });
        return instance.GetType().FullName;
    }

    public Type[] MethodParameters { get; }
    public Type SourceType { get; }
    public string SourceMethodName { get; }
    public bool IsBefore { get; }
    public bool IsReturn { get; }
}