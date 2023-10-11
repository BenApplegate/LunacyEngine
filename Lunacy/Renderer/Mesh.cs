
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class Mesh
{
    private List<float> verticies = new List<float>();
    private List<float> triangleIndicies = new List<float>();
    public bool isDynamic;

    private bool isGenerated = false;
    internal int _VBO;
    internal int _VAO;
    

    public Mesh(bool dynamic = false)
    {
        isDynamic = dynamic;
    }

    public void AddVertex(Vector3 location)
    {
        verticies.Add(location.X);
        verticies.Add(location.Y);
        verticies.Add(location.Z);
    }

    public void UpdateMeshData()
    {
        foreach (float f in verticies)
        {
            Logger.Warning(f.ToString());
        }
        if (!isGenerated)
        {
            _VAO = GL.GenVertexArray();
            _VBO = GL.GenBuffer();

            isGenerated = true;
        }
        //Bind our VAO
        GL.BindVertexArray(_VAO);
        
        //Load Buffer Data
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, verticies.Count * sizeof(float), verticies.ToArray(), isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        //Unbind array to save data
        GL.BindVertexArray(0);
    }

    public void Bind()
    {
        GL.BindVertexArray(_VAO);
    }
}

