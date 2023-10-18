using System.Data;
using System.Reflection;
using System.Resources;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer;

public class Shader
{
    private Vector4 _albedo = Vector4.One;
    private Texture _albedoTexture;
    private List<Texture> _textures = new List<Texture>();
    private Matrix4 _transform = Matrix4.Identity;
    private int _programHandle;
    private bool _disposed = false;
    private Shader()
    {
        
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            GL.DeleteProgram(_programHandle);
            _disposed = true;
        }
    }

    public void Attach()
    {
        GL.UseProgram(_programHandle);
    }

    public void BindTextures()
    {
        if (_albedoTexture is not null)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _albedoTexture._imageHandle);
        }

        for (int i = 0; i < _textures.Count; i++)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + i + 1);
            GL.BindTexture(TextureTarget.Texture2D, _textures[i]._imageHandle);
        }
    }

    ~Shader()
    {
        //We cannot dispose here because its possible for the GL context to no longer exist
        //But we can give a warning to the user
        if (!_disposed)
        {
            Logger.Error("Shader went out of scope without being disposed. Please call Dispose() on your shader" +
                         "to avoid GPU resource leaks");
        }
    }

    public static Shader DefaultUnlit()
    {
        Shader s = new Shader();
        int _vertexShader; 
        int _fragmentShader;
        
        //Load and compile Vertex Shader
        Stream? vertexShaderStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Lunacy.Resources.Shaders.Unlit.unlit.vertex");
        if (vertexShaderStream == null)
        {
            Logger.Error("Could not find unlit shader in engine resources");
            throw new MissingManifestResourceException();
        }
        string vertexSource = new StreamReader(vertexShaderStream).ReadToEnd();
        _vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(_vertexShader, 1, new []{vertexSource}, new []{vertexSource.Length});
        GL.CompileShader(_vertexShader);
        
        //Check if vertex shader compiled successfully
        int success;
        GL.GetShader(_vertexShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Vertex Shader failed to compile: \"{GL.GetShaderInfoLog(_vertexShader)}\"");
            throw new SyntaxErrorException();
        }
        
        
        //Load and compile Fragment Shader
        Stream? fragmentShaderStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Lunacy.Resources.Shaders.Unlit.unlit.frag");
        if (fragmentShaderStream == null)
        {
            Logger.Error("Could not find unlit shader in engine resources");
            throw new MissingManifestResourceException();
        }
        string fragmentSource = new StreamReader(fragmentShaderStream).ReadToEnd();
        _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(_fragmentShader, 1, new []{fragmentSource}, new []{fragmentSource.Length});
        GL.CompileShader(_fragmentShader);
        
        //Check if vertex shader compiled successfully
        GL.GetShader(_fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Vertex Shader failed to compile: \"{GL.GetShaderInfoLog(_fragmentShader)}\"");
            throw new SyntaxErrorException();
        }
        
        
        //Now we need to link the shader program
        s._programHandle = GL.CreateProgram();
        GL.AttachShader(s._programHandle, _vertexShader);
        GL.AttachShader(s._programHandle, _fragmentShader);
        GL.LinkProgram(s._programHandle);
        
        //Check if program linked correctly
        GL.GetProgram(s._programHandle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Shader program failed to link: \"{GL.GetProgramInfoLog(s._programHandle)}\"");
            throw new Exception("Shader Program failed to link");
        }
        
        
        //We now have a linked shader program, we can delete the shader handles
        GL.DeleteShader(_vertexShader);
        GL.DeleteShader(_fragmentShader);
        
        Logger.Info("Finished Compiling and Linking shader program");

        s.SetAlbedo(Vector4.One);
        
        return s;
    }

    public static Shader FromStreams(Stream vertexShaderStream, Stream fragmentShaderStream)
    {
        Shader s = new Shader();
        int _vertexShader; 
        int _fragmentShader;
        
        //Load and compile Vertex Shader
        if (vertexShaderStream == null)
        {
            Logger.Error("Vertex Shader stream is null");
            throw new NullReferenceException();
        }
        string vertexSource = new StreamReader(vertexShaderStream).ReadToEnd();
        _vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(_vertexShader, 1, new []{vertexSource}, new []{vertexSource.Length});
        GL.CompileShader(_vertexShader);
        
        //Check if vertex shader compiled successfully
        int success;
        GL.GetShader(_vertexShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Vertex Shader failed to compile: \"{GL.GetShaderInfoLog(_vertexShader)}\"");
            throw new SyntaxErrorException();
        }
        
        
        //Load and compile Fragment Shader
        if (fragmentShaderStream == null)
        {
            Logger.Error("Fragment Shader Stream is null");
            throw new NullReferenceException();
        }
        string fragmentSource = new StreamReader(fragmentShaderStream).ReadToEnd();
        _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(_fragmentShader, 1, new []{fragmentSource}, new []{fragmentSource.Length});
        GL.CompileShader(_fragmentShader);
        
        //Check if vertex shader compiled successfully
        GL.GetShader(_fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Vertex Shader failed to compile: \"{GL.GetShaderInfoLog(_fragmentShader)}\"");
            throw new SyntaxErrorException();
        }
        
        
        //Now we need to link the shader program
        s._programHandle = GL.CreateProgram();
        GL.AttachShader(s._programHandle, _vertexShader);
        GL.AttachShader(s._programHandle, _fragmentShader);
        GL.LinkProgram(s._programHandle);
        
        //Check if program linked correctly
        GL.GetProgram(s._programHandle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Shader program failed to link: \"{GL.GetProgramInfoLog(s._programHandle)}\"");
            throw new Exception("Shader Program failed to link");
        }
        
        
        //We now have a linked shader program, we can delete the shader handles
        GL.DeleteShader(_vertexShader);
        GL.DeleteShader(_fragmentShader);
        
        Logger.Info("Finished Compiling and Linking shader program");
        
        s.SetAlbedo(Vector4.One);
        
        return s;
    }

    public void SetVec4(string uniformName, Vector4 color)
    {
        int uniformLocation = GL.GetUniformLocation(_programHandle, uniformName);
        if (uniformLocation == -1)
        {
            Logger.Warning($"{uniformName} is not a valid uniform name for this shader");
            return;
        }
        GL.UseProgram(_programHandle);
        GL.Uniform4(uniformLocation, color);
    }

    public void SetAlbedo(Vector4 albedo)
    {
        _albedo = albedo;
        Attach();
        int uniformLocation = GL.GetUniformLocation(_programHandle, "albedo");
        if (uniformLocation == -1)
        {
            Logger.Warning($"Shader does not have albedo uniform");
            return;
        }
        GL.UseProgram(_programHandle);
        GL.Uniform4(uniformLocation, _albedo);
    }

    internal void SetTransformMatrix(Matrix4 transform)
    {
        _transform = transform;
        Attach();
        int uniformLocation = GL.GetUniformLocation(_programHandle, "transform");
        if (uniformLocation == -1)
        {
            Logger.Warning($"Shader does not have transform matrix uniform");
            return;
        }
        GL.UseProgram(_programHandle);
        GL.UniformMatrix4(uniformLocation, false, ref _transform);
    }

    public void SetAlbedoTexture(Texture albedoTexture)
    {
        this._albedoTexture = albedoTexture;
        Attach();
        int uniformLocation = GL.GetUniformLocation(_programHandle, "albedoTexture");
        if (uniformLocation == -1)
        {
            Logger.Warning($"Shader does not have albedo texture uniform");
            return;
        }
        GL.Uniform1(uniformLocation, 0);
    }

    public void SetTexture(Texture texture, string location)
    {
        if (_textures.Count == 15)
        {
            Logger.Warning("You have run out of available texture slots for this shader, texture did not attatch");
            return;
        }
        _textures.Add(texture);
        Attach();
        int textureID = _textures.Count;
        int uniformLocation = GL.GetUniformLocation(_programHandle, location);
        if (uniformLocation == -1)
        {
            Logger.Warning($"Shader does not have \"{location}\" uniform");
            return;
        }
        GL.Uniform1(uniformLocation, textureID);

    }
    
}