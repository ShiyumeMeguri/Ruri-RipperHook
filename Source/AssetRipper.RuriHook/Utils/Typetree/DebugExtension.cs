using AssetRipper.SourceGenerated.Classes.ClassID_25;

namespace AssetRipper.RuriHook;
public static class DebugExtension
{
	/// <summary>
	/// 子类查找器
	/// </summary>
	public static void SubClassFinder()
	{
		Type baseType = typeof(Renderer_2017_3_0);
		string targetAssemblyName = "AssetRipper.SourceGenerated";
		string targetNamespace = "AssetRipper.SourceGenerated.Classes";
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
