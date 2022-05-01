using System;
using System.Collections.Generic;
using UnityEngine;

namespace OsnovaFramework
{
    public sealed class Entity : MonoBehaviour
    {
        static readonly Dictionary<int, Entity> entities = new();
        
        public int Id { get; private set; }

        readonly Dictionary<Type, BaseComponent> components = new ();
        
        void Awake()
        {
            Id = gameObject.GetHashCode();

            var startComponents = GetComponents<BaseComponent>();

            foreach (var component in startComponents)
                AddExisting(component);
            
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
        
        public T Add<T>() where T : BaseComponent
        {
            var component = Get<T>();

            if (!component)
            {
                component = gameObject.AddComponent<T>();
                AddExisting(component);
            }

            return component;
        }

        T AddExisting<T>(T component) where T : BaseComponent
        {
            component.SetEntity(this);
            components.Add(component.GetType(), component);
            BaseComponent.Register(component);

            return component;
        }
        
        public void Remove<T>() where T : BaseComponent
        {
            var component = Get<T>();

            if (component)
                Destroy(component);
        }

        public T GetSignal<T>() where T : Signal => Signal.Get<T>(Id);
        public bool AddSignal(Signal signal) => Signal.Add(Id, signal);
        public bool AddSignal<T>() where T : Signal, new () => AddSignal(new T());

        public static void ResetAll() => entities.Clear();

        public static Entity GetEntity(GameObject byObject)
        {
            entities.TryGetValue(byObject.GetHashCode(), out var entity);

            return entity;
        }
        
        public static T GetComponent<T>(int entityId) where T : BaseComponent => entities[entityId].Get<T>();
        public static bool HasComponent<T>(int entityId) where T : BaseComponent => entities[entityId].Has<T>();
        
        public static Entity GetEntity(int id)
        {
            entities.TryGetValue(id, out var entity);

            return entity;
        }

        static void Register(Entity entity) => entities.Add(entity.Id, entity);

        static void Unregister(Entity entity)
        {
            entities.Remove(entity.Id);
            Signal.UnregisterEntity(entity.Id);

            foreach (var keyValuePair in entity.components)
                BaseComponent.Unregister(keyValuePair.Value);
        }
    }
}