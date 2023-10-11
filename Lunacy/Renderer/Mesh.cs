
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class Mesh
{
    private List<float> verticies = new List<float>();
    private List<uint> triangleIndicies = new List<uint>();
    public bool isDynamic;

    private bool isGenerated = false;
    internal int _VBO;
    internal int _VAO;
    internal int _EBO;
    
    public Mesh(bool dynamic = false)
    {
        isDynamic = dynamic;
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_VBO);
        GL.DeleteBuffer(_EBO);
        GL.DeleteVertexArray(_VAO);
    }

    public void SetVerticies(List<float> verticies)
    {
        this.verticies = verticies;
    }

    public void SetIndicies(List<uint> indicies)
    {
        this.triangleIndicies = indicies;
    }

    public void UpdateMeshData()
    {
        if (!isGenerated)
        {
            _VAO = GL.GenVertexArray();
            _VBO = GL.GenBuffer();
            _EBO = GL.GenBuffer();

            isGenerated = true;
        }
        //Bind our VAO
        GL.BindVertexArray(_VAO);
        
        //Load Buffer Data
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, verticies.Count * sizeof(float), verticies.ToArray(), isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, triangleIndicies.Count * sizeof(uint), triangleIndicies.ToArray(), isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
        
        //Unbind array to save data
        GL.BindVertexArray(0);
    }

    public void Bind()
    {
        GL.BindVertexArray(_VAO);
    }

    internal int GetIndiciesCount() => triangleIndicies.Count;
}

