using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace Lunacy.Core;

public static class LunacyEngine
{
    internal static GameWindow _window;
    private static Scene _currentScene;

    private static Thread _renderThread;
    private static Queue<GameObject> _readyToRender;
    private static bool _stopRender = false;
    private static bool _windowShouldClose = false;
    
    
    //Render state of zero means objects still need to update
    //Render state of one means objects still need to render
    //Render state of three means buffers are ready to swap
    private static short _renderState = 0;

    public static void Initialize(Scene scene, int width = 800, int height = 600, string windowTitle = "Lunacy Game", Version? openGLVersion = null)
    {
        Logger.Initialize();
        _currentScene = scene;
        _renderThread = new Thread(RenderThread);
        _readyToRender = new Queue<GameObject>();
        
        Logger.Info("Creating Engine Windows");

        if (openGLVersion == null) openGLVersion = new Version(3, 3);
        
        //Create window settings from passed parameters and create window
        var windowSettings = new NativeWindowSettings() { Size = (width, height), Title = windowTitle,
            API = ContextAPI.OpenGL, APIVersion = openGLVersion};
        _window = new GameWindow(GameWindowSettings.Default, windowSettings);
        
        //Setup callback for window close event
        _window.Closing += args =>
        {
            _windowShouldClose = true;
        };

        _window.Resize += args =>
        {
            GL.Viewport(0, 0, args.Width, args.Height);
        };
        
        if (openGLVersion == null)
        {
            openGLVersion = new Version(3, 3);
        }
        
        _window.MakeCurrent();
        GL.Viewport(0, 0, width, height);
        
        Logger.Info("Lunacy Engine successfully initialized");
    }
    
    public static void Run()
    {
        // _renderThread.Start();

        //Event Loop
        Logger.Info("Starting Engine Event Loop");
        while (!_windowShouldClose)
        {
            _window.ProcessEvents(1);
            
            //Clear Buffers and reset internal render state
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(.1f, .1f, .1f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _renderState = 0;

            //Loop over all objects and update them
            foreach (GameObject obj in _currentScene.GetSceneObjects())
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

    public static void Dispose()
    {
        Logger.Warning("Engine has been disposed, Do not attempt to use engine until reinitialized");
        _window.Dispose();
        Logger.Dispose();
    }
}