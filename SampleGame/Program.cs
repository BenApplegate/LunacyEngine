using System.Reflection;
using Lunacy.Core;
using Lunacy.Renderer;
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
        Shader triangleShader = Shader.DefaultUnlit();
        Mesh triangleMesh = new Mesh();
        Texture milkmanTexture = Texture.FromColor(100, 255, 10);

        triangleShader.SetAlbedoTexture(milkmanTexture);

        triangleMesh.SetVerticies(new List<float>(new float[]{
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left 
        }));
        
        triangleMesh.SetIndicies(new List<uint>(new uint[]
        {
            0, 1, 3,
            1, 2, 3
        }));
        
        triangleMesh.SetUVCoordinates(new List<float>(new float[]
        {
            1, 1,
            1, 0,
            0, 0,
            0, 1
        }));
        
        triangleMesh.UpdateMeshData();
        obj.AddComponent(new MeshRenderer2D(triangleMesh, triangleShader));
        obj.AddComponent(new ObjectSettingsEditor());

        LunacyEngine.Run();
        
        milkmanTexture.Dispose();
        triangleShader.Dispose();
        triangleMesh.Dispose();
        LunacyEngine.Dispose();
    }
}