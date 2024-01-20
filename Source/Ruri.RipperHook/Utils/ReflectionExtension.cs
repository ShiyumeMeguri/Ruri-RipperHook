using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ruri.RipperHook;
public static class ReflectionExtension
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
}
