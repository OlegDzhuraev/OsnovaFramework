using System.Collections.Generic;
using UnityEngine;

namespace OsnovaFramework
{
    public static class Entities
    {
        static readonly List<Entity> entities = new();
        
        public static void Register(Entity entity) => entities.Add(entity);
        public static void Unregister(Entity entity) => entities.Remove(entity);

        public static void Reset() => entities.Clear();
        
        public static Entity GetEntity(this GameObject go) => entities.Find(e => e.gameObject == go);
        public static Entity GetEntity(this MonoBehaviour mb) => entities.Find(e => e.gameObject == mb.gameObject);
    }
}