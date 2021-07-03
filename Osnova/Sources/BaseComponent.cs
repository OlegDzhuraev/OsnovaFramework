using UnityEngine;

namespace OsnovaFramework
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        
        internal void SetEntity(Entity entity) => Entity = entity;

        public T Get<T>() where T : BaseComponent => Entity.Get<T>();
        
        void Awake() => Components.Register(this);
        void OnDestroy() => Components.Unregister(this);
    }
}