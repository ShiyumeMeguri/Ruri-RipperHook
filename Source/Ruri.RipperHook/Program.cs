using System;
using System.CommandLine;

namespace Ruri.RipperHook;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Hook(args);
        RunAssetRipper();
    }

    private static void Hook(string[] args)
    {
        RootCommand rootCommand = new() { Description = "Example command line application" };

        var hookOption = new Option<string>(
            aliases: ["-h", "--hooks"],
            description: "A list of hooks separated by commas",
            getDefaultValue: () => string.Empty);

        rootCommand.AddOption(hookOption);

        rootCommand.SetHandler((string hooks) =>
        {
            foreach (var hook in hooks.Split(','))
            {
                if (Enum.TryParse<GameHookType>(hook, true, out var hookType))
                {
                    RuriRuntimeHook.Init(hookType);
                }
                else
                {
                    Console.WriteLine($"Invalid hook type. Currently supported:\n{string.Join("\n", Enum.GetNames(typeof(GameHookType)))}");
                    Environment.Exit(1);
                }
            }
        }, hookOption);

        if (rootCommand.Invoke(args) != 1) return;
        Console.WriteLine($"Invalid hook type. Currently supported:\n{string.Join("\n", Enum.GetNames(typeof(GameHookType)))}");
        Environment.Exit(1);
    }

    private static void RunAssetRipper()
    {
        var programType = Type.GetType("AssetRipper.GUI.Program, AssetRipper");
        var mainMethod = programType.GetMethod("Main", ReflectionExtensions.AnyBindFlag());
        object[] parameters = { new string[0] };
        mainMethod.Invoke(null, parameters);
    }
}