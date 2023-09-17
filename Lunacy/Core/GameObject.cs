using System.Numerics;
using Lunacy.Components;

namespace Lunacy.Core;

public class GameObject
{
    private string _name;
    private Scene _scene;
    
    //Transform information
    public Vector3 location = Vector3.Zero;
    public Vector3 rotation = Vector3.Zero;
    public Vector3 scale = Vector3.One;

    private List<Component> _components;

    public GameObject(Scene parentScene, string name)
    {
        this._name = name;
        parentScene.AddSceneObject(this);
    }

    public string GetName() => _name;
    public void SetName(string name) => this._name = name;

    public void AddComponent(Component component) => _components.Add(component);

    public T[] GetComponents<T>() where T : class, Component
    {
        return Array.ConvertAll(_components.Where(component => component is T).ToArray(), c => (T)c);
    }

    public T? GetComponent<T>() where T : class, Component
    {
        var matches = GetComponents<T>();
        if (matches.Length > 0) return matches.First();
        return null;
    }
}