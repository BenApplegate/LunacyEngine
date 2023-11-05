using System.Diagnostics;
using ImGuiNET;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Lunacy.Core;

public static class LunacyEngine
{
    internal static GameWindow _window;
    internal static LunacyEngineSettings _engineSettings;
    
    private static Scene _currentScene;

    private static Thread _renderThread;
    private static Queue<GameObject> _readyToRender;
    private static bool _stopRender = false;
    private static bool _windowShouldClose = false;

    private static float _aspectRatio;

    private static ImGuiController _imGuiController;
    
    
    //Render state of zero means objects still need to update
    //Render state of one means objects still need to render
    //Render state of three means buffers are ready to swap
    private static short _renderState = 0;

    public static void Initialize(LunacyEngineSettings engineSettings, Scene currentScene)
    {
        Logger.Initialize();
        _currentScene = currentScene;
        _renderThread = new Thread(RenderThread);
        _readyToRender = new Queue<GameObject>();

        Logger.Info("Creating Engine Windows");
        
        _aspectRatio = (float)engineSettings.WindowSize.X / engineSettings.WindowSize.Y;
        _engineSettings = engineSettings;

        if (engineSettings.OpenGLVersion == null)
            engineSettings.OpenGLVersion = new Version(3, 3);

        NativeWindowSettings windowSettings = new NativeWindowSettings
        {
            Title = engineSettings.Title,
            Size = engineSettings.WindowSize,
            API = ContextAPI.OpenGL,
            APIVersion = engineSettings.OpenGLVersion,
            Icon = engineSettings.WindowIcon,
            Location = engineSettings.WindowLocation,
            MaximumSize = engineSettings.MaximumSize,
            Vsync = engineSettings.VSync,
            WindowBorder = engineSettings.WindowBorder,
            WindowState = engineSettings.WindowState,
        };
        _engineSettings.NativeWindowSettings = windowSettings;

        _window = new GameWindow(GameWindowSettings.Default, windowSettings);
        
        //Setup callback for window close event
        _window.Closing += args =>
        {
            _windowShouldClose = true;
        };
        
        _window.Resize += args =>
        {
            _aspectRatio = (float)args.Width / args.Height;
            GL.Viewport(0, 0, args.Width, args.Height);
            
            _imGuiController.WindowResized(args.Width, args.Height);
        };

        _window.TextInput += args =>
        {
            _imGuiController.PressChar((char)args.Unicode);
        };

        _window.MouseWheel += args =>
        {
            _imGuiController.MouseScroll(args.Offset);
        };

        _window.MakeCurrent();
        
        _imGuiController = new ImGuiController(engineSettings.WindowSize.X, engineSettings.WindowSize.Y);
        
        GL.Viewport(0, 0, engineSettings.WindowSize.X, engineSettings.WindowSize.Y);
        
        Logger.Info("Lunacy Engine successfully initialized");

    }

    public static void Initialize(Scene scene, int width = 800, int height = 600, string windowTitle = "Lunacy Game", Version? openGLVersion = null)
    {
        Initialize(new LunacyEngineSettings
        {
            Title = windowTitle,
            WindowSize = (width, height),
            OpenGLVersion = openGLVersion,
        }, scene);
    }
    
    public static void Run()
    {
        // _renderThread.Start();

        //Event Loop
        Logger.Info("Starting Engine Event Loop");
        
        Stopwatch frameTimes = Stopwatch.StartNew();
        while (!_windowShouldClose)
        {
            _window.ProcessEvents(0);

            //Clear Buffers and reset internal render state
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(.1f, .1f, .1f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _renderState = 0;

            _imGuiController.Update(_window, frameTimes.ElapsedMilliseconds/1000f);
            frameTimes.Restart();
            
            //Loop over all objects and update them
            foreach (GameObject obj in _currentScene.RefSceneObjects())
            {
                obj.Update();
                //After an object finishes updating, add it to render queue
                lock (_readyToRender)
                {
                    _readyToRender.Enqueue(obj);
                }
            }
            _renderState = 1;

            while (_renderState != 2)
            {
                //Wait for Render thread to finish if needed
                _readyToRender.Dequeue().Render();
                if (_readyToRender.Count == 0) _renderState = 2;
            }

            _imGuiController.Render();
            
            _window.SwapBuffers();
        }

        _stopRender = true;
    }

    private static void RenderThread()
    {
        _window.MakeCurrent();
        while (!_stopRender)
        {
            if (_renderState == 2)
            {
                continue;
            }
            if (_readyToRender.Count > 0)
            {
                GameObject rendering;
                lock (_readyToRender)
                {
                    rendering = _readyToRender.Dequeue();
                }
                
                rendering.Render();
            }

            if (_readyToRender.Count == 0 && _renderState == 1)
            {
                //Update thread has marked all objects as being done updated
                //Render thread has emptied the queue, buffer should be ready to swap
                _renderState = 2;
                
            }
        }

        Logger.Warning("Render Thread Stopping");
    }

    public static bool GetKeyDown(Keys key)
    {
        return _window.IsKeyPressed(key);
    }

    public static float GetAspectRatio()
    {
        return _aspectRatio;
    }

    public static void Dispose()
    {
        Logger.Warning("Engine has been disposed, Do not attempt to use engine until reinitialized");
        _imGuiController.Dispose();
        _window.Dispose();
        Logger.Dispose();
    }
}