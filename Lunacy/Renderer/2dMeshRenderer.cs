using Lunacy.Components;
using Lunacy.Core;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

        Matrix4 transform =
            Matrix4.CreateTranslation(gameObject.location)
                                             * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(gameObject.rotation))
                                             * Matrix4.CreateScale(new Vector3(1 / LunacyEngine.GetAspectRatio(), 1, 1)) 
                                             * Matrix4.CreateScale(gameObject.scale);
        _shader.SetTransformMatrix(transform);
        
        //Logger.Info("Rendering");
        _shader.Attach();
        _shader.BindTextures();
        _mesh.Bind();
        GL.DrawElements(BeginMode.Triangles, _mesh.GetIndiciesCount(), DrawElementsType.UnsignedInt, 0);
    }

    public Shader GetShader()
    {
        return _shader;
    }
}