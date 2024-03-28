using System.Diagnostics;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodAttribute : Attribute
{
    public RetargetMethodAttribute(Type sourceType, string sourceMethodName = "ReadRelease", int maxArgIndex = 1, bool isBefore = true, bool isReturn = true, Type[] methodParameters = null)
    {
        SourceType = sourceType;
        SourceMethodName = sourceMethodName;
        MaxArgIndex = maxArgIndex;
        IsBefore = isBefore;
        IsReturn = isReturn;
        MethodParameters = methodParameters;
    }

    public RetargetMethodAttribute(string sourceTypeName, string sourceMethodName = "ReadRelease", int maxArgIndex = 1, bool isBefore = true, bool isReturn = true, Type[] methodParameters = null)
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        SourceMethodName = sourceMethodName;
        MaxArgIndex = maxArgIndex;
        IsBefore = isBefore;
        IsReturn = isReturn;
        MethodParameters = methodParameters;
    }

    public Type[] MethodParameters { get; }
    public Type SourceType { get; }
    public string SourceMethodName { get; }
    public int MaxArgIndex { get; }
    public bool IsBefore { get; }
    public bool IsReturn { get; }
}