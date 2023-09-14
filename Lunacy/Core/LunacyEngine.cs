using OpenTK.Windowing.Desktop;

namespace Lunacy.Core;

public static class LunacyEngine
{
    internal static GameWindow _window;
    
    public static void Initialize(int width = 800, int height = 600, string windowTitle = "Lunacy Game")
    {
        //Create window settings from passed parameters and create window
        var windowSettings = new NativeWindowSettings() { Size = (width, height), Title = windowTitle};
        _window = new GameWindow(GameWindowSettings.Default, windowSettings);
        
        //Setup callback for window close event
        bool windowShouldClose = false;
        _window.Closing += args =>
        {
            windowShouldClose = true;
        };
        
        //Event Loop
        while (!windowShouldClose)
        {
            _window.ProcessEvents(1);
        }
    }

    public static void Dispose()
    {
        _window.Dispose();
    }
}