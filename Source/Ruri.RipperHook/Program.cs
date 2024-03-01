using AssetRipper.GUI;
using AssetRipper.Import.Logging;
using Avalonia;
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

        var hookOption = new Option<string>(aliases:["-h", "--hooks"], description: "A list of hooks separated by commas",
        getDefaultValue:
        () => string.Empty);

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
        Logger.Add(new FileLogger());
        Logger.Add(new ConsoleLogger());
        Logger.LogSystemInformation("AssetRipper GUI Version");
        BuildAvaloniaApp().Start(App.AppMain, Array.Empty<string>());
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>().UsePlatformDetect().With(new X11PlatformOptions
        {
            UseDBusFilePicker = false
            //Disable FreeDesktop file picker
            //https://github.com/AvaloniaUI/Avalonia/issues/9383
        }).LogToTrace();
    }
}