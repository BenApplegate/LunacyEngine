using Lunacy.Components;
using Lunacy.Utils;

namespace SampleGame;

public class TestComponent : Component
{
    public override void OnAwake()
    {
        Logger.Info("Awake Has RUN!");
        
    }

    public override void OnUpdate()
    {
        Logger.Info($"Update has been called for {gameObject.GetName()}");
    }

    public override void OnRender()
    {
        Logger.Info($"Render has been called for {gameObject.GetName()}");
    }
}