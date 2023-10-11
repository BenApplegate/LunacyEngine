using Lunacy.Core;

namespace Lunacy.Components;

public class Component
{
    protected internal GameObject gameObject {  get;  internal set; }
    
    public virtual void OnAwake(){}
    public virtual void OnUpdate(){}
    public virtual void OnRender(){}
    
}