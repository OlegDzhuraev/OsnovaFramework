using UnityEngine;

namespace OsnovaFramework
{
    public abstract class BaseSystem : MonoBehaviour
    {
        public Layer Layer { get; private set; }
        
        public void Setup(Layer layer) => Layer = layer;
        public virtual void Init() { }
        public abstract void Run(); 
    }
}