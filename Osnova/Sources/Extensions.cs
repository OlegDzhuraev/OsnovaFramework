using UnityEngine;

namespace OsnovaFramework
{
    public static class Extensions
    {
        public static Entity GetEntity(this GameObject gameObject) => Entity.GetEntity(gameObject);
        public static Entity GetEntity(this Transform transform) => Entity.GetEntity(transform.gameObject);
        public static Entity GetEntity(this MonoBehaviour monoBehaviour) => Entity.GetEntity(monoBehaviour.gameObject);
    }
}