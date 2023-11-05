using Lunacy.Components;
using Lunacy.Core;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class MeshRenderer3D : Component{
    private Mesh _mesh;
    private Shader _shader;
    private Camera _camera;

    public MeshRenderer3D(Mesh mesh, Shader shader, Camera activeCamera)
    {
        _mesh = mesh;
        _shader = shader;
        _camera = activeCamera;
    }

    public override void OnRender()
    {
        if (_mesh.isDynamic)
        {
            _mesh.UpdateMeshData();
        }

        Matrix4 model =
            Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(gameObject.rotation))
            * Matrix4.CreateScale(gameObject.scale)
            * Matrix4.CreateTranslation(gameObject.location);

        Matrix4 view = _camera.GetViewMatrix();
        
        Matrix4 projectionMatrix = _camera.GetProjectionMatrix();
        
        _shader.SetTransformMatrix(model * view * projectionMatrix);
        
        //Logger.Info("Rendering");
        _shader.Attach();
        _shader.BindTextures();
        _mesh.Bind();
        GL.DrawElements(BeginMode.Triangles, _mesh.GetIndiciesCount(), DrawElementsType.UnsignedInt, 0);
        _shader.UnbindTextures();
    }

    public Shader GetShader()
    {
        return _shader;
    }

    public void SetActiveCamera(Camera camera)
    {
        _camera = camera;
    }
}