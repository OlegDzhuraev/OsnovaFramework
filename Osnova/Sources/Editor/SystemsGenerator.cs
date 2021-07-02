using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OsnovaFramework.Editor
{
    public static class SystemsGenerator
    {
        [MenuItem("Osnova Framework/Generate Systems assets")]
        static void GenerateSystemsAssets()
        {
            var systemsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BaseSystem)) && !type.IsAbstract);

            if (!AssetDatabase.IsValidFolder("Assets/OsnovaGenerated"))
                AssetDatabase.CreateFolder("Assets","OsnovaGenerated");

            var result = "";
            var generatedAmount = 0;
            
            foreach (var type in systemsTypes)
            {
                var path = $"Assets/OsnovaGenerated/{type.Name}.asset";

                if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(path) is not null)
                    continue;
                
                var so = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(so, path);

                generatedAmount++;
                result += type.Name + "\n";
            }
            
            AssetDatabase.SaveAssets();
            
            Debug.Log($"[Osnova Framework] Generated {generatedAmount} systems\n\n" + result);
        }
    }
}