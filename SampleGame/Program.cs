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
        triangleMesh.AddVertex(new Vector3(-.5f, -.5f, 0f));
        triangleMesh.AddVertex(new Vector3(.5f, -.5f, 0f));
        triangleMesh.AddVertex(new Vector3(0f, .5f, 0f));
        triangleMesh.UpdateMeshData();
        obj.AddComponent(new MeshRenderer2D(triangleMesh, triangleShader));

        LunacyEngine.Run();
        
        LunacyEngine.Dispose();
    }
}