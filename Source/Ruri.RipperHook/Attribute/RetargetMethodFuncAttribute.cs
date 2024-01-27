using System.Diagnostics;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodFuncAttribute : Attribute
{
    public RetargetMethodFuncAttribute(Type sourceType, string sourceMethodName = "ReadRelease")
    {
        SourceType = sourceType;
        SourceMethodName = sourceMethodName;
    }

    public RetargetMethodFuncAttribute(string sourceTypeName, string sourceMethodName = "ReadRelease")
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        SourceMethodName = sourceMethodName;
    }

    public Type SourceType { get; }
    public string SourceMethodName { get; }
}