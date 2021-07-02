using System;
using System.Collections.Generic;
using System.Linq;

namespace OsnovaFramework
{
    public static class Components
    {
        static readonly Dictionary<Type, List<BaseComponent>> components = new ();

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
        
        static List<BaseComponent> GetComponentsList<T>() => GetComponentsList(typeof(T));

        public static void Register(BaseComponent component) =>
            GetComponentsList(component.GetType()).Add(component);
        
        public static void Unregister(BaseComponent component) =>
            GetComponentsList(component.GetType()).Remove(component);

        public static Filter<T> Filter<T>() where T : BaseComponent => 
            new (GetComponentsList<T>().Select(c => c as T));

        public static T GetFirst<T>() where T : BaseComponent
        {
            var typedComponents = GetComponentsList<T>();

            if (typedComponents.Count > 0)
                return typedComponents[0] as T;

            return null;
        }

        public static void Reset() => components.Clear();
    }
}