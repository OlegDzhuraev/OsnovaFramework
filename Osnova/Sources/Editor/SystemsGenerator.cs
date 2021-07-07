using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OsnovaFramework.Editor
{
    public static class SystemsGenerator
    {
        const string assetsPath = "Assets/OsnovaGenerated";
        const string debugPrefix = "[Osnova Framework]";
        
        [MenuItem("Osnova Framework/Generate Systems assets")]
        static void GenerateSystemsAssets()
        {
            var systemsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BaseSystem)) && !type.IsAbstract);

            if (!AssetDatabase.IsValidFolder(assetsPath))
                AssetDatabase.CreateFolder("Assets","OsnovaGenerated");

            var result = "";
            var generatedAmount = 0;
            
            foreach (var type in systemsTypes)
            {
                var path = $"{assetsPath}/{type.Name}.asset";
                var existingAssetPath = GetAssetPathByType(type);
                
                if (!string.IsNullOrEmpty(existingAssetPath))
                {
                    if (existingAssetPath != path)
                    {
                        var renameResult = AssetDatabase.RenameAsset(existingAssetPath, type.Name);
                        
                        if (string.IsNullOrEmpty(renameResult))
                            Debug.Log($"{debugPrefix} Fixed naming for {existingAssetPath} to {type.Name}.");
                    }

                    continue;
                }

                var so = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(so, path);

                generatedAmount++;
                result += type.Name + "\n";
            }
            
            AssetDatabase.SaveAssets();
            
            Debug.Log($"{debugPrefix} Generated {generatedAmount} systems\n\n" + result);
        }
        
        static string GetAssetPathByType(Type type)
        {
            var guids = AssetDatabase.FindAssets($"t:{type}");

            if (guids.Length == 0)
                return null;
            
            var guid = guids[0];
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                
            return asset != null ? assetPath : null;
        }
    }
}