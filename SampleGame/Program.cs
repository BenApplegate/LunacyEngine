using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Utils;
using OpenTK.Mathematics;

namespace SampleGame;

public class Program
{
    public static void Main(string[] args)
    {
        Scene scene = new Scene();
        LunacyEngine.Initialize(scene);
        
        GameObject obj = new GameObject(scene, "Test Triangle");
        Shader triangleShader = Shader.DefaultUnlit();
        Mesh triangleMesh = new Mesh();
        
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
        
        triangleMesh.UpdateMeshData();
        obj.AddComponent(new MeshRenderer2D(triangleMesh, triangleShader));
        obj.AddComponent(new ColorChanger());

        LunacyEngine.Run();
        
        triangleShader.Dispose();
        LunacyEngine.Dispose();
    }
}