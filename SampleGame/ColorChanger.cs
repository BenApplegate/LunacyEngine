using Lunacy.Components;
using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SampleGame;

public class ColorChanger : Component
{
    public override void OnUpdate()
    {
        if (LunacyEngine.GetKeyDown(Keys.Space))
        {
            //Logger.Info("Count has hit 500, updating color");
            Random random = new Random();
            MeshRenderer2D renderer = gameObject.GetComponent<MeshRenderer2D>()!;
            renderer.GetShader().SetAlbedo(new Vector4(random.NextSingle(),random.NextSingle(),random.NextSingle(),1));
            gameObject.scale = new Vector3(random.NextSingle(), random.NextSingle(), random.NextSingle());
            gameObject.rotation = new Vector3(random.NextSingle() * 360, random.NextSingle() * 360, random.NextSingle() * 360);
            gameObject.location = new Vector3((2 * random.NextSingle() - 1), (2 * random.NextSingle() - 1),
                0);
            
            Logger.Info($"Location: {gameObject.location}, Rotation: {gameObject.rotation}, Scale: {gameObject.scale}");

        }
    }
}