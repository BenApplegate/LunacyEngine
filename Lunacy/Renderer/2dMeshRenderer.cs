using Lunacy.Components;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;

namespace Lunacy.Renderer;

public class MeshRenderer2D : Component
{
    private Mesh _mesh;
    private Shader _shader;

    public MeshRenderer2D(Mesh mesh, Shader shader)
    {
        _mesh = mesh;
        _shader = shader;
    }

    public override void OnRender()
    {
        if (_mesh.isDynamic)
        {
            _mesh.UpdateMeshData();
        }
        //Logger.Info("Rendering");
        _shader.Attach();
        _mesh.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}