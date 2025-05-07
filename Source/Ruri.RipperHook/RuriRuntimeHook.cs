using MonoMod.RuntimeDetour;
using Ruri.RipperHook.Crypto;

namespace Ruri.RipperHook;

/// <summary>
/// Hook到方法体时当前this相当于变成了方法所在的类
/// 因此不能在类里添加任何成员 否则会访问到错误的内存
/// </summary>
public static class RuriRuntimeHook
{
    public static List<ILHook> ilHooks = new();
    public static Dictionary<GameHookType, RipperHook> currentGameHook = new();
    public static string gameName;
    public static string gameVer;
    
    // 原因如上 禁止在任何Hook类中添加成员字段所以只能写全局
    public static CommonDecryptor commonDecryptor;
    public static UnityChinaDecryptor unityChinaDecryptor;

    public static void Init(GameHookType gameName)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        InstallHook(gameName);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Initialization {gameName} completed Current Hooks Count {ilHooks.Count}");
        Console.ResetColor();
    }

    public static void DestoyHook(ILHook iLHook)
    {
        iLHook.Dispose();
        RuriRuntimeHook.ilHooks.Remove(iLHook);
    }

    private static void InstallHook(GameHookType hookName)
    {
        var name = hookName.ToString();
        if (!name.StartsWith("AR_"))
        {
            var na = name.Split('_');
            gameName = na[0];
            gameVer = string.Join(".", na.Skip(1));
        }

        var type = Type.GetType("Ruri.RipperHook." + name + "." + name + "_Hook");
        currentGameHook[hookName] = (RipperHook)Activator.CreateInstance(type, true);
    }
}