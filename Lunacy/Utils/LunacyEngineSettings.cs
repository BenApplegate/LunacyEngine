using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace Lunacy.Utils;

public class LunacyEngineSettings
{
    /// <summary>
    /// Gets or sets the title of the new window.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the initial size of the contents of the window.
    /// </summary>
    public Vector2i WindowSize { get; set; } = (800, 600);
    
    /// <summary>
    /// Gets or sets the current OpenGL version for the engine
    /// </summary>
    public Version? OpenGLVersion { get; set; }

    /// <summary>
    /// Gets or sets the current WindowIcon for this window.
    /// </summary>
    public WindowIcon? WindowIcon { get; set; } = null;

    /// <summary>
    /// Gets or sets the location to open the new window on.
    /// </summary>
    public Vector2i? WindowLocation { get; set; } = null;

    /// <summary>
    /// Gets or sets the maximum size of the contents of the window.
    /// </summary>
    public Vector2i? MaximumSize { get; set; } = null;
    
    /// <summary>
    /// Gets or sets a value indicating the vsync mode to use. A pure NativeWindow supports Off and On.
    /// GameWindow adds support for Adaptive, if you are not using GameWindow you will have to handle adaptive vsync yourself.
    /// </summary>
    public VSyncMode VSync { get; set; }

    /// <summary>
    /// Gets or sets the initial value for WindowBorder on the new window.
    /// </summary>
    public WindowBorder WindowBorder { get; set; }

    /// <summary>
    /// Gets or sets the initial value for WindowState on the new window.
    /// This setting is ignored if StartVisible = false.
    /// </summary>
    public WindowState WindowState { get; set; }

    /// <summary>
    /// Gets or sets the NativeWindowSettings for OpenTK
    /// </summary>
    internal NativeWindowSettings NativeWindowSettings { get; set; }






}