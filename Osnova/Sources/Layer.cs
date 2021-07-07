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
            
            GlobalSignal.ClearAll();
            Signal.ClearAll();
        }
    }
}