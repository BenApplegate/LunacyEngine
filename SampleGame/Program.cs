using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Utils;

namespace SampleGame;

public class Program
{
    public static void Main(string[] args)
    {
        LunacyEngine.Initialize(new Scene());

        
        var shader = Shader.DefaultUnlit();
        shader.Attach();
        shader.Dispose();
        
        LunacyEngine.Dispose();
    }
}