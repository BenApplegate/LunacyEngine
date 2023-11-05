using System.Reflection;
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
        obj.AddComponent(new MeshRenderer2D(Mesh.cube, triangleShader));
        obj.AddComponent(new ObjectSettingsEditor());

        LunacyEngine.Run();
        
        texture.Dispose();
        triangleShader.Dispose();
        LunacyEngine.Dispose();
    }
}