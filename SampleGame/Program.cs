using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Utils;
using OpenTK.Windowing.Common;

namespace SampleGame;

public class Program
{
    public static void Main(string[] args)
    {
        Scene scene = new Scene();

        LunacyEngineSettings settings = new LunacyEngineSettings()
        {
            Title = "Sample Lunacy Game",
            WindowBorder = WindowBorder.Fixed,
        };

        LunacyEngine.Initialize(settings, scene);
        LunacyEngine.Run();
        
        LunacyEngine.Dispose();
    }
}