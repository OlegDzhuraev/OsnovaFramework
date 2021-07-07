using System;
using System.Collections.Generic;

namespace OsnovaFramework
{
    public abstract class Signal
    {
        static readonly Dictionary<int, Dictionary<Type, Signal>> signals = new ();

        public static bool Add(int entityId, Signal signal)
        {
            if (!CanReceiveSignals(entityId))
                MakeSignalListener(entityId);
            
            var type = signal.GetType();
            signals.TryGetValue(entityId, out var entitySignals);
            
            if (entitySignals.ContainsKey(type))
                return false;
            
            entitySignals.Add(type, signal);
            return true;
        }
        
        public static T Get<T>(int entityId) where T : Signal
        {
            if (!CanReceiveSignals(entityId))
                return null;
            
            signals.TryGetValue(entityId, out var entitySignals);
            entitySignals.TryGetValue(typeof(T), out var signal);

            return signal as T;
        }

        public static void UnregisterEntity(int entityId) => signals.Remove(entityId);

        public static void ClearAll()
        {
            foreach (var entitySignals in signals)
                entitySignals.Value.Clear();
        }
        
        public static implicit operator bool(Signal signal) => signal != null;
        
        static bool CanReceiveSignals(int entityId) => signals.ContainsKey(entityId);
        static void MakeSignalListener(int entityId) => signals.Add(entityId, new Dictionary<Type, Signal>());
    }
}