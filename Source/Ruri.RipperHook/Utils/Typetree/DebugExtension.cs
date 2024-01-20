using AssetRipper.SourceGenerated.Classes.ClassID_25;

namespace Ruri.RipperHook;
public static class DebugExtension
{
	/// <summary>
	/// 子类查找器
	/// </summary>
	public static void SubClassFinder(Type baseType, string targetAssemblyName, string targetNamespace)
	{
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			if (assembly.GetName().Name == targetAssemblyName)
			{
				var typesInNamespace = assembly.GetTypes()
					.Where(t => t.Namespace != null && t.Namespace.StartsWith(targetNamespace));
				foreach (var type in typesInNamespace)
				{
					if (type.IsSubclassOf(baseType))
					{
						Console.WriteLine("Found subclass: " + type.FullName);
					}
				}
				break;
			}
		}
	}
}
