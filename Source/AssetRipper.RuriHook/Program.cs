using System.Reflection;

namespace AssetRipper.RuriHook;

static class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		RuriRuntimeHook.Init(GameHookType.ShaderDecompiler);
		Type programType = Type.GetType("AssetRipper.GUI.Program, AssetRipper");
		MethodInfo mainMethod = programType.GetMethod("Main", ReflectionExtension.AnyBindFlag());
		object[] parameters = new object[] { args };
		mainMethod.Invoke(null, parameters);
	}
}
