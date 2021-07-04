using System;
using System.Collections.Generic;
using UnityEngine;

namespace OsnovaFramework
{
    [DisallowMultipleComponent]
    public sealed class Layer : MonoBehaviour
    {
        [SerializeField] List<BaseSystem> systems = new ();

        void Start()
        {
            foreach (var system in systems)
                system.Start();
        }

        void Update()
        {
            foreach (var system in systems)
                system.Update();
            
            GlobalSignal.ResetAll();
            Entity.ResetEntitiesSignals();
        }

        public bool HaveSystem<T>() => systems.Find(s => s is T);
        public bool HaveSystem(Type systemType) => systems.Find(s => s.GetType() == systemType);
    }
}