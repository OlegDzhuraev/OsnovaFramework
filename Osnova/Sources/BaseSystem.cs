using UnityEngine;

namespace OsnovaFramework
{
    public abstract class BaseSystem : MonoBehaviour
    {
        public Layer Layer { get; private set; }
        
        public void Init(Layer layer) => Layer = layer;
        public virtual void Start() { }
        public abstract void Run(); 
    }
}