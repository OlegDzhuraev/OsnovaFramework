using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OsnovaFramework
{
    [RequireComponent(typeof(Entity))]
    public abstract class BaseComponent : MonoBehaviour
    {
        static readonly Dictionary<Type, List<BaseComponent>> components = new ();
        
        public Entity Entity { get; private set; }

        public T Get<T>() where T : BaseComponent => Entity.Get<T>();
        internal void SetEntity(Entity entity) => Entity = entity;
        
        public static void Register(BaseComponent component) => GetComponentsList(component.GetType()).Add(component);
        public static void Unregister(BaseComponent component) => GetComponentsList(component.GetType()).Remove(component);

        public static Filter<T> Filter<T>() where T : BaseComponent => 
            new (GetComponentsList<T>().Select(c => c as T));

        public static void ResetAll() => components.Clear();
        
        static List<BaseComponent> GetComponentsList(Type byType)
        {
            var tryGetValue = components.TryGetValue(byType, out var componentsList);

            if (!tryGetValue)
            {
                componentsList = new List<BaseComponent>();
                components.Add(byType, componentsList);
            }

            return componentsList;
        }
        
        static IEnumerable<BaseComponent> GetComponentsList<T>() => GetComponentsList(typeof(T));
    }
}