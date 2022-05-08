using System;
using System.Collections.Generic;
using UnityEngine;

namespace OsnovaFramework
{
    [DisallowMultipleComponent]
    public sealed class Layer : MonoBehaviour
    {
        [SerializeField] List<BaseSystem> systems = new ();
        [SerializeField] ScriptableObject[] settings = Array.Empty<ScriptableObject>();
        [SerializeField] LayerRunType runType = LayerRunType.Update; 

        #if UNITY_EDITOR
        void OnValidate()
        {
            var objectSystems = gameObject.GetComponents<BaseSystem>();

            foreach (var system in objectSystems)
                if (!systems.Contains(system))
                    systems.Add(system);

            for (var i = systems.Count - 1; i >= 0; i--)
                if (systems[i] == null)
                    systems.RemoveAt(i);
        }
        #endif

        void Start()
        {
            foreach (var system in systems)
            {
                system.Setup(this);
                system.Init();
            }
        }

        void Update()
        {
            if (runType == LayerRunType.Update)
                Run();
        }

        void FixedUpdate()
        {
            if (runType == LayerRunType.FixedUpdate)
                Run();
        }

        public void Run()
        {
            foreach (var system in systems)
                system.Run();
            
            GlobalSignal.ClearAll();
            Signal.ClearAll();
        }
        
        public T GetSettings<T>() where T : ScriptableObject
        {
            foreach (var so in settings)
                if (so is T typedSo)
                    return typedSo;

            return null;
        }
    }
}