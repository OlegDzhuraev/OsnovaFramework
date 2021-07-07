using System;
using System.Collections.Generic;

namespace OsnovaFramework
{
    public static class GlobalSignal
    {
        static readonly Dictionary<Type, Signal> signals = new();
        
        public static T Get<T>() where T : Signal
        {
            signals.TryGetValue(typeof(T), out var signal);

            return signal as T;
        }

        public static bool Add(Signal signal)
        {
            var type = signal.GetType();

            if (!signals.ContainsKey(type))
                return false;

            signals.Add(type, signal);
            return true;
        }
        
        public static bool Add<T>() where T : Signal, new () => Add(new T());
        public static void ClearAll() => signals.Clear();
    }
}