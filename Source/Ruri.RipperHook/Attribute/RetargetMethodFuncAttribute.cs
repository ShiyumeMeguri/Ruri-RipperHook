using System.Diagnostics;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodFuncAttribute : Attribute
{
    public RetargetMethodFuncAttribute(Type sourceType, string sourceMethodName = "ReadRelease", Type[] methodParameters = null)
    {
        SourceType = sourceType;
        SourceMethodName = sourceMethodName;
        MethodParameters = methodParameters;
    }

    public RetargetMethodFuncAttribute(string sourceTypeName, string sourceMethodName = "ReadRelease", Type[] methodParameters = null)
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        SourceMethodName = sourceMethodName;
        MethodParameters = methodParameters;
    }

    public Type[] MethodParameters { get; }
    public Type SourceType { get; }
    public string SourceMethodName { get; }
}