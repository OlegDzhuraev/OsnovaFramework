using UnityEngine;

namespace OsnovaFramework
{
    public abstract class BaseSystem : ScriptableObject
    {
        public virtual void Start() { }
        public abstract void Update();
    }
}