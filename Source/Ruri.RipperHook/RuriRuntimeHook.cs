using Ruri.RipperHook.Crypto;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Ruri.RipperHook;
public static class RuriRuntimeHook
{
	public static List<ILHook> ilHooks = new List<ILHook>();
	public static Dictionary<GameHookType, AssetHook> currentGameHook = new Dictionary<GameHookType, AssetHook>();
	public static GameCrypto gameCrypto;
	public static void Init(GameHookType gameName)
	{
		Console.ForegroundColor = ConsoleColor.Blue;

		InstallHook(gameName);

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"Initialization {gameName} completed Current Hooks Count {ilHooks.Count}");
		Console.ResetColor();
	}
	private static void InstallHook(GameHookType hookName)
	{
		string name = hookName.ToString();
		Type type = Type.GetType("Ruri.RipperHook." + name + "." + name + "_Hook");
		currentGameHook[hookName] = (AssetHook)Activator.CreateInstance(type, true);
	}
	public static void RetargetCallFunc(Func<ILContext, bool> func, MethodInfo srcMethod)
	{
		var hookDest = new ILContext.Manipulator(il =>
		{
			if (!func(il))
				throw new Exception($"Hook {srcMethod.DeclaringType.Name}.{srcMethod.Name} Fail");

		});
		ilHooks.Add(new ILHook(srcMethod, hookDest));
		Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
	}
	public static void RetargetCall(MethodInfo srcMethod, MethodInfo targetMethod, int argsCount = 1, bool isBefore = true, bool isReturn = true)
	{
		var hookDest = new ILContext.Manipulator(il =>
		{
			ILCursor ilCursor = new ILCursor(il);
			if (!isBefore) // 从起点注入还是末尾注入
				while (ilCursor.TryGotoNext(MoveType.Before, instr => instr.OpCode == OpCodes.Ret)) ;
			for (int i = 0; i <= argsCount; i++)
			{
				switch (i)
				{
					case 0:
						ilCursor.Emit(OpCodes.Ldarg_0);
						continue;
					case 1:
						ilCursor.Emit(OpCodes.Ldarg_1);
						continue;
					case 2:
						ilCursor.Emit(OpCodes.Ldarg_2);
						continue;
					case 3:
						ilCursor.Emit(OpCodes.Ldarg_3);
						continue;
					default:
						ilCursor.Emit(OpCodes.Ldarg, i);
						continue;
				}
			}
			ilCursor.Emit(OpCodes.Call, targetMethod);
			if (isReturn)
				ilCursor.Emit(OpCodes.Ret);
		});

		ilHooks.Add(new ILHook(srcMethod, hookDest));
		Console.WriteLine($"Created Hook of {srcMethod.DeclaringType.Name}.{srcMethod.Name} Success");
	}
}
