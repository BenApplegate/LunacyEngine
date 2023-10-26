
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class Mesh
{
    private List<float> verticies = new List<float>();
    private List<uint> triangleIndicies = new List<uint>();
    private List<float> UVCoordinates = new List<float>();
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
        if (isGenerated)
        {
            GL.DeleteBuffer(_EBO);
            GL.DeleteBuffer(_VBO);
            GL.DeleteVertexArray(_VAO);
            isGenerated = false;
        }
    }

    public void SetVerticies(List<float> verticies)
    {
        this.verticies = verticies;
    }

    public void SetIndicies(List<uint> indicies)
    {
        this.triangleIndicies = indicies;
    }

    public void SetUVCoordinates(List<float> coordinates)
    {
        this.UVCoordinates = coordinates;
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
        
        if (verticies.Count % 3 != 0)
        {
            Logger.Error("Mesh Vertex locations length must be a multiple of 3 as each location is an X, Y, and Z coordinate");
            throw new Exception();
        }

        if (UVCoordinates.Count % 2 != 0)
        {
            Logger.Error("Mesh UV coordinates length must be a multiple of 2 as each location is a U, and V coordinate");
            throw new Exception();
        }
        
        //We must combine our locations and UVs into one buffer for openGL
        List<float> assembledVBO = new List<float>();
        int vertIndex = 0;
        int uvIndex = 0;

        while (vertIndex < verticies.Count && uvIndex < UVCoordinates.Count)
        {
            assembledVBO.Add(verticies[vertIndex + 0]);
            assembledVBO.Add(verticies[vertIndex + 1]);
            assembledVBO.Add(verticies[vertIndex + 2]);
            vertIndex += 3;
            assembledVBO.Add(UVCoordinates[uvIndex + 0]);
            assembledVBO.Add(UVCoordinates[uvIndex + 1]);
            uvIndex += 2;
        }

        if (vertIndex < verticies.Count)
        {
            Logger.Warning("There are more verticies in the mesh than UV coordinates, filling remaining coordinates with 0s");
        }
        while (vertIndex < verticies.Count)
        {
            assembledVBO.Add(verticies[vertIndex + 0]);
            assembledVBO.Add(verticies[vertIndex + 1]);
            assembledVBO.Add(verticies[vertIndex + 2]);
            assembledVBO.Add(0);
            assembledVBO.Add(0);
            vertIndex += 3;
        }
        
        //Load Buffer Data
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, assembledVBO.Count * sizeof(float), assembledVBO.ToArray(), isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

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

