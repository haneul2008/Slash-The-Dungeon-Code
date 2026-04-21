using System.Collections.Generic;
using System.Text;
using HN.Code.Weapon.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Core.Editor
{
    public abstract class SoEnumGenerator<TSo, TEnum> : EnumGenerator<TEnum> where TSo : ScriptableObject where TEnum : System.Enum
    {
        [SerializeField] protected string dataPath;
        
        protected override StringBuilder GetCodeBuilder()
        {
            StringBuilder codeBuilder = new StringBuilder();
            
            string[] guidArray = AssetDatabase.FindAssets($"t:{typeof(TSo).Name}", new string[]{dataPath});
            
            foreach (string guid in guidArray)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TSo data = AssetDatabase.LoadAssetAtPath<TSo>(path);
                codeBuilder.Append($"{data.name}, ");
            }
            
            return codeBuilder;
        }
    }
}