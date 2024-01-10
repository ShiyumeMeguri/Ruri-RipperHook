using AssetRipper.Assets;
using AssetRipper.Assets.Generics;
using AssetRipper.IO.Endian;
using MonoMod.Cil;
using System.Reflection;

namespace AssetRipper.RuriHook;
public abstract class AssetHook
{
	protected List<string> additionalNamespaces = new List<string>();
	protected AssetHook()
	{
		InitAttributeHook();
	}
	protected virtual void InitAttributeHook()
	{
		var bindingFlags = ReflectionExtension.AnyBindFlag();
		var namespacesToConsider = new List<string> { GetType().Namespace };
		namespacesToConsider.AddRange(additionalNamespaces); // 添加一些通用空间 避免写编写重复代码

		var methods = Assembly.GetExecutingAssembly().GetTypes()
					.Where(t => t.Namespace != null && namespacesToConsider.Any(ns => t.Namespace.StartsWith(ns)))
					.SelectMany(t => t.GetMethods(bindingFlags));

		var targetMethods = methods.Where(m => m.GetCustomAttributes<RetargetMethodAttribute>(true).Any());
		foreach (var methodDest in targetMethods)
		{
			var attrs = methodDest.GetCustomAttributes<RetargetMethodAttribute>().ToArray();
			foreach (var attr in attrs)
			{
				var methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags);
				RuriRuntimeHook.RetargetCall(methodSrc, methodDest, attr.ArgCount);
			}
		}

		var targetFuncMethods = methods.Where(m => m.GetCustomAttributes<RetargetMethodFuncAttribute>(true).Any());
		foreach (var methodDest in targetFuncMethods)
		{
			var attrs = methodDest.GetCustomAttributes<RetargetMethodFuncAttribute>().ToArray();
			foreach (var attr in attrs)
			{
				var methodSrc = attr.SourceType.GetMethod(attr.SourceMethodName, bindingFlags);
				var HookCallback = (Func<ILContext, bool>)Delegate.CreateDelegate(typeof(Func<ILContext, bool>), methodDest);
				RuriRuntimeHook.RetargetCallFunc(HookCallback, methodSrc);
			}
		}
	}

	protected void SetPrivateField(Type type, string name, object newValue)
	{
		type.GetField(name, ReflectionExtension.PrivateInstanceBindFlag()).SetValue(this, newValue);
	}
	protected object GetPrivateField(Type type, string name)
	{
		return type.GetField(name, ReflectionExtension.PrivateInstanceBindFlag()).GetValue(this);
	}
	protected void SetAssetListField<T>(Type type, string name, ref EndianSpanReader reader, bool isAlign = true) where T : UnityAssetBase, new()
	{
		FieldInfo field = type.GetField(name, ReflectionExtension.PrivateInstanceBindFlag());

		Type fieldType = field.FieldType;
		object filedObj = Activator.CreateInstance(fieldType);
		if (isAlign)
			((AssetList<T>)filedObj).ReadRelease_ArrayAlign_Asset(ref reader);
		else
			((AssetList<T>)filedObj).ReadRelease_Array_Asset(ref reader);

		field.SetValue(this, filedObj);
	}
}
