using OpenTK.Windowing.Desktop;

namespace LunacyEngine.Core;

public static class LunacyEngine
{
    internal static GameWindow _window;
    
    public static void Initialize(int width = 800, int height = 600, string windowTitle = "Lunacy Game")
    {
        var windowSettings = new NativeWindowSettings() { Size = (width, height), Title = windowTitle};
        _window = new GameWindow(GameWindowSettings.Default, windowSettings);
    }

    public static void Dispose()
    {
        _window.Dispose();
    }
}