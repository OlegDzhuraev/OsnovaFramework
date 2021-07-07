using System.Collections.Generic;
using UnityEngine;

namespace OsnovaFramework
{
    [CreateAssetMenu(fileName = "OsnovaSettings", menuName = "Osnova Framework/Settings", order = 0)]
    public sealed class Settings : ScriptableObject
    {
        public static Settings Instance
        {
            get
            {
                if (!instance)
                    instance = Resources.Load<Settings>("OsnovaSettings");

                return instance;
            }
        }
        
        static Settings instance;
        
        [SerializeField] List<ScriptableObject> settingsAssets = new ();

        public static T Get<T>() where T : ScriptableObject
        {
            foreach (var so in Instance.settingsAssets)
                if (so is T typedSo)
                    return typedSo;

            return null;
        }
    }
}