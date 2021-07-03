using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OsnovaFramework
{
    public sealed class Entity : MonoBehaviour
    {
        static int GetUniqueId => actualId++;
        static int actualId;

        static readonly List<Entity> entities = new();

        public int Id { get; private set; }

        readonly Dictionary<Type, BaseComponent> components = new ();

        void Awake()
        {
            Id = GetUniqueId;

            var list = GetComponents<BaseComponent>().ToList();

            foreach (var baseComponent in list)
            {
                components.Add(baseComponent.GetType(), baseComponent);
                baseComponent.SetEntity(this);
            }

            Register(this);
        }
        
        void OnDestroy() => Unregister(this);

        public T Get<T>() where T : BaseComponent
        {
            components.TryGetValue(typeof(T), out var component);

            if (component is T typedComponent)
                return typedComponent;
            
            return null;
        }

        public bool Has<T>() where T : BaseComponent => Get<T>();

        public static void Register(Entity entity) => entities.Add(entity);
        public static void Unregister(Entity entity) => entities.Remove(entity);
        
        public static void ResetEntities() => entities.Clear();
        
        public static Entity GetEntity(GameObject byObject) => entities.Find(e => e.gameObject == byObject);
    }
}