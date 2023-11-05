using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Lunacy.Renderer;

public class Texture
{
    private ImageResult _image;
    internal int _imageHandle;

    private Texture()
    {
        
    }

    public void Dispose()
    {
        GL.DeleteTexture(_imageHandle);
    }

    public static Texture LoadFromStream(Stream stream, TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear, TextureMagFilter magFilter = TextureMagFilter.Linear)
    {
        Texture t = new Texture
        {
            _imageHandle = GL.GenTexture()
        };

        GL.BindTexture(TextureTarget.Texture2D, t._imageHandle);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);
        
        StbImage.stbi_set_flip_vertically_on_load(1);
        t._image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, t._image.Width, t._image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, t._image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return t;

    }

    public void UpdateFromStream(Stream stream, TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear, TextureMagFilter magFilter = TextureMagFilter.Linear)
    {
        GL.BindTexture(TextureTarget.Texture2D, _imageHandle);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);

        StbImage.stbi_set_flip_vertically_on_load(1);
        _image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _image.Width, _image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, _image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }

    public static Texture FromColor(byte r, byte g, byte b, byte a = 255, TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear, TextureMagFilter magFilter = TextureMagFilter.Linear)
    {
        Texture t = new Texture
        {
            _imageHandle = GL.GenTexture()
        };

        GL.BindTexture(TextureTarget.Texture2D, t._imageHandle);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);

        byte[] data = new[] { r, g, b, a };
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 1, 1, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return t;
    }
}