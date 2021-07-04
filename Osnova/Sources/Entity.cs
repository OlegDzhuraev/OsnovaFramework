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
        readonly Dictionary<Type, Signal> signals = new ();

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
        
        public T Add<T>() where T : BaseComponent
        {
            var component = Get<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }
        
        public void Remove<T>() where T : BaseComponent
        {
            var component = Get<T>();

            if (component)
                Destroy(component);
        }

        public T GetSignal<T>() where T : Signal
        {
            signals.TryGetValue(typeof(T), out var signal);

            return signal as T;
        }

        public bool AddSignal(Signal signal)
        {
            var type = signal.GetType();
            
            if (signals.ContainsKey(type))
                return false;
            
            signals.Add(type, signal);
            
            return true;
        }
        
        public bool AddSignal<T>() where T : Signal, new () => AddSignal(new T());
        
        void ResetSignals() => signals.Clear();
        
        public static void ResetEntities() => entities.Clear();
        public static void ResetEntitiesSignals() => entities.ForEach(e => e.ResetSignals());
        
        public static Entity GetEntity(GameObject byObject) => entities.Find(e => e.gameObject == byObject);

        static void Register(Entity entity) => entities.Add(entity);
        static void Unregister(Entity entity) => entities.Remove(entity);
    }
}