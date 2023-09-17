namespace Lunacy.Core;

public class Scene
{
    private List<GameObject> _sceneObjects = new List<GameObject>();

    public List<GameObject> GetSceneObjects()
    {
        //Return copy of scene objects list
        //This prevents component code from unintentionally
        //removing objects from the scene
        return new List<GameObject>(_sceneObjects);
    }

    internal void AddSceneObject(GameObject obj)
    {
        _sceneObjects.Add(obj);
    }
}