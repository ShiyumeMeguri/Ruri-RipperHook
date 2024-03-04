using System.Diagnostics;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodCtorFuncAttribute : Attribute
{
    public RetargetMethodCtorFuncAttribute(Type sourceType, Type[] methodParameters = null)
    {
        SourceType = sourceType;
        MethodParameters = methodParameters;
    }

    public RetargetMethodCtorFuncAttribute(string sourceTypeName, Type[] methodParameters = null)
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        MethodParameters = methodParameters;
    }

    public Type[] MethodParameters { get; }
    public Type SourceType { get; }
}