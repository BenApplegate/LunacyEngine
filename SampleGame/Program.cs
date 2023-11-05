using System.Reflection;
using Lunacy.Components;
using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Renderer.Shaders;
using Lunacy.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace SampleGame;

public class Program
{
    public static void Main(string[] args)
    {
        LunacyEngineSettings settings = new LunacyEngineSettings
        {
            Title = "Sample Lunacy Project",
            WindowState = WindowState.Normal,
            WindowBorder = WindowBorder.Resizable,
        };
        
        Scene scene = new Scene();
        LunacyEngine.Initialize(settings, scene);
        
        GameObject obj = new GameObject(scene, "Test Triangle");
        Texture texture = Texture.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("SampleGame.Resources.Images.face.png"));
        Shader triangleShader = new UnlitShader(texture);

        obj.scale = new Vector3(.5f, .5f, .5f);

        GameObject camObject = new GameObject(scene, "camera");
        Camera cam = new Camera();
        camObject.AddComponent(cam);
        camObject.AddComponent(new ObjectSettingsEditor());
        
        obj.AddComponent(new MeshRenderer3D(Mesh.cube, triangleShader, cam));
        obj.AddComponent(new ObjectSettingsEditor());

        LunacyEngine.Run();
        
        texture.Dispose();
        triangleShader.Dispose();
        LunacyEngine.Dispose();
    }
}