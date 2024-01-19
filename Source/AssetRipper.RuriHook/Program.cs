using System.Reflection;

namespace AssetRipper.RuriHook;

static class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		Hook();
		RunAssetRipper(args);
	}
	private static void Hook()
	{
		RuriRuntimeHook.Init(GameHookType.ShaderDecompiler);
		RuriRuntimeHook.Init(GameHookType.Houkai_7_1);
	}
	private static void RunAssetRipper(string[] args)
	{
		Type programType = Type.GetType("AssetRipper.GUI.Program, AssetRipper");
		MethodInfo mainMethod = programType.GetMethod("Main", ReflectionExtension.AnyBindFlag());
		object[] parameters = new object[] { args };
		mainMethod.Invoke(null, parameters);
	}
}
