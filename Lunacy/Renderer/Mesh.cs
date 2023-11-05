
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class Mesh
{
    public static Mesh quad;
    public static Mesh cube;
    
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

    internal static void InitDefaultMeshes()
    {
        //INIT QUAD
        quad = new Mesh();
        quad.SetVerticies(new List<float>(new float[]{
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left 
        }));
        
        quad.SetIndicies(new List<uint>(new uint[]
        {
            0, 1, 3,
            1, 2, 3
        }));
        
        quad.SetUVCoordinates(new List<float>(new float[]
        {
            1, 1,
            1, 0,
            0, 0,
            0, 1
        }));
        
        quad.UpdateMeshData();
        
        
        //INIT CUBE
        cube = new Mesh();
        cube.SetVerticies(new List<float>(new float[]
        {
            -1, -1, 1, //Front Bottom Left 0
            1, -1, 1, //Front Bottom Right 1
            -1, 1, 1, //Front Top Left 2
            1, 1, 1, //Front Top Right 3
            
            1, -1, 1, //Right Bottom Left 4
            1, -1, -1, //Right Bottom Right 5
            1, 1, 1, //Right Top Left 6
            1, 1, -1, //Right Top Right 7
            
            1, -1, -1, //Back Bottom Left 8
            -1, -1, -1, //Back Bottom Right 9
            1, 1, -1, //Back Top Left 10
            -1, 1, -1, //Back Top Right 11
            
            -1, -1, -1, //Left Bottom Left 12
            -1, -1, 1, //Left Bottom Right 13
            -1, 1, -1, //Left Top Left 14
            -1, 1, 1, //Left Top Right 15
            
            -1, 1, 1, // Top Bottom Left 16
            1, 1, 1, //Top Bottom Right 17
            -1, 1, -1, //Top Top Left 18
            1, 1, -1, //Top Top Right 19
            
            -1, -1, 1, //Bottom Top Left 20
            1, -1, 1, //Bottom Top Right 21
            -1, -1, -1, //Bottom Bottom Left 22
            1, -1, -1 // Bottom Bottom Right 23
            
        }));
        
        cube.SetIndicies(new List<uint>(new uint[]
        {
            //Front face
            0, 1, 2,
            1, 3, 2,
            
            //Right face
            4, 5, 6,
            5, 7, 6,
            
            //Back Face
            8, 9, 10,
            9, 11, 10,
            
            //Left Face
            12, 13, 14,
            13, 15, 14,
            
            //Top Face
            16, 17, 18,
            17, 19, 18,
            
            //Bottom Face
            22, 23, 20,
            23, 21, 20
        }));

        cube.SetUVCoordinates(new List<float>(new float[]
        {
            0, 0,
            1, 0,
            0, 1,
            1, 1,
            
            0, 0,
            1, 0,
            0, 1,
            1, 1,
            
            0, 0,
            1, 0,
            0, 1,
            1, 1,
            
            0, 0,
            1, 0,
            0, 1,
            1, 1,
            
            0, 0,
            1, 0,
            0, 1,
            1, 1,
            
            0, 0,
            1, 0,
            0, 1,
            1, 1,
        }));
        
        cube.UpdateMeshData();
    }

    internal static void DisposeDefaultMeshes()
    {
        quad.Dispose();
        cube.Dispose();
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

