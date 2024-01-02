using System.Diagnostics;

namespace AssetRipper.RuriHook
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class RetargetMethodFuncAttribute : Attribute
	{
		public Type SourceType { get; private set; }
		public string SourceMethodName { get; private set; }
		public RetargetMethodFuncAttribute(Type sourceType, string sourceMethodName = "ReadRelease")
		{
			SourceType = sourceType;
			SourceMethodName = sourceMethodName;
		}
		public RetargetMethodFuncAttribute(string sourceTypeName, string sourceMethodName = "ReadRelease")
		{
			SourceType = Type.GetType(sourceTypeName);
			Debug.Assert(SourceType!=null);
			SourceMethodName = sourceMethodName;
		}
	}
}
