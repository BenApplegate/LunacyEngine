using Lunacy.Core;

namespace SampleGame;

public class Program
{
    public static void Main(string[] args)
    {
        Scene scene = new Scene();
        for (int i = 0; i < 50; i++)
        {
            GameObject testObject = new GameObject(scene, $"test_object_{i}");
            testObject.AddComponent(new TestComponent());
        }
        
        
        LunacyEngine.Initialize(scene);
        LunacyEngine.Run();
        
        
        LunacyEngine.Dispose();
    }
}