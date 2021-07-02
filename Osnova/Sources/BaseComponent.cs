using UnityEngine;

namespace OsnovaFramework
{
    public class BaseComponent : MonoBehaviour
    {
        public Entity Entity { get; private set; }
        
        internal void SetEntity(Entity entity) => Entity = entity;
        
        void Awake() => Components.Register(this);
        void OnDestroy() => Components.Unregister(this);
    }
}