using System.Diagnostics;

namespace Ruri.RipperHook;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RetargetMethodAttribute : Attribute
{
    public RetargetMethodAttribute(Type sourceType,
        string sourceMethodName = "ReadRelease",
        int argCount = 1,
        bool isBefore = true,
        bool isReturn = true)
    {
        SourceType = sourceType;
        SourceMethodName = sourceMethodName;
        ArgCount = argCount;
        IsBefore = isBefore;
        IsReturn = isReturn;
    }

    public RetargetMethodAttribute(string sourceTypeName,
        string sourceMethodName = "ReadRelease",
        int argCount = 1,
        bool isBefore = true,
        bool isReturn = true)
    {
        SourceType = Type.GetType(sourceTypeName);
        Debug.Assert(SourceType != null);
        SourceMethodName = sourceMethodName;
        ArgCount = argCount;
        IsBefore = isBefore;
        IsReturn = isReturn;
    }

    public Type SourceType { get; }
    public string SourceMethodName { get; }
    public int ArgCount { get; }
    public bool IsBefore { get; }
    public bool IsReturn { get; }
}