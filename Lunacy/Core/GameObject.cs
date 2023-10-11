
using Lunacy.Components;
using OpenTK.Mathematics;

namespace Lunacy.Core;

public class GameObject
{
    private string _name;
    private Scene _scene;
    private GameObject? _parent;
    
    //Transform information
    public Vector3 location = Vector3.Zero;
    public Vector3 rotation = Vector3.Zero;
    public Vector3 scale = Vector3.One;

    private List<Component> _components;
    private List<GameObject> _children;

    public GameObject(Scene parentScene, string name)
    {
        this._name = name;
        _components = new List<Component>();
        _children = new List<GameObject>();
        parentScene.AddSceneObject(this);
    }

    public void AddChild(GameObject child)
    {
        _children.Add(child);
        child._parent = this;
    }

    public List<GameObject> FindChildrenWithName(string name)
    {
        return _children.Where(obj => obj._name == name).ToList();
    }

    public string GetName() => _name;
    public void SetName(string name) => this._name = name;

    public void AddComponent(Component component)
    {
        _components.Add(component);
        component.gameObject = this;
        component.OnAwake();
    }

    internal void Update()
    {
        foreach (var c in _components)
        {
            c.OnUpdate();
        }
    }

    internal void Render()
    {
        foreach (var c in _components)
        {
            c.OnRender();
        }
    }

    public T[] GetComponents<T>() where T : Component
    {
        return Array.ConvertAll(_components.Where(component => component is T).ToArray(), c => (T)c);
    }

    public T? GetComponent<T>() where T : Component
    {
        var matches = GetComponents<T>();
        if (matches.Length > 0) return matches.First();
        return null;
    }
}