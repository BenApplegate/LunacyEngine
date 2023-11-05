using System.Data;
using System.Reflection;
using System.Resources;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lunacy.Renderer.Shaders;

public class UnlitShader : Shader
{
    private Vector4 _tint;
    private Texture _albedo;
    private bool _shouldDisposeTexture;

    public override void Dispose()
    {
        if (_shouldDisposeTexture)
        {
            _albedo.Dispose();
        }
        base.Dispose();
    }

    private void Init()
    {
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
        this._programHandle = GL.CreateProgram();
        GL.AttachShader(this._programHandle, _vertexShader);
        GL.AttachShader(this._programHandle, _fragmentShader);
        GL.LinkProgram(this._programHandle);
        
        //Check if program linked correctly
        GL.GetProgram(this._programHandle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            Logger.Error($"Shader program failed to link: \"{GL.GetProgramInfoLog(this._programHandle)}\"");
            throw new Exception("Shader Program failed to link");
        }
        
        
        //We now have a linked shader program, we can delete the shader handles
        GL.DeleteShader(_vertexShader);
        GL.DeleteShader(_fragmentShader);
        
        Logger.Info("Finished Compiling and Linking shader program");
    }
    
    public UnlitShader()
    {
        Init();

        _albedo = Texture.FromColor(255, 255, 255);
        _shouldDisposeTexture = true;
        SetTexture(_albedo, "albedoTexture");
        SetTint(Vector4.One);
    }

    public UnlitShader(byte r, byte b, byte g, byte a = 255)
    {
        Init();

        _albedo = Texture.FromColor(r, b, g, a);
        _shouldDisposeTexture = true;
        SetTexture(_albedo, "albedoTexture");
        SetTint(Vector4.One);
    }

    public UnlitShader(Texture albedo)
    {
        Init();

        _albedo = albedo;
        _shouldDisposeTexture = false;
        SetTexture(_albedo, "albedoTexture");
        SetTint(Vector4.One);
    }
    
    public void SetTint(Vector4 tint)
    {
        _tint = tint;
        Attach();
        int uniformLocation = GL.GetUniformLocation(_programHandle, "tint");
        if (uniformLocation == -1)
        {
            Logger.Warning($"Shader does not have albedo uniform");
            return;
        }
        GL.UseProgram(_programHandle);
        GL.Uniform4(uniformLocation, _tint);
    }
}