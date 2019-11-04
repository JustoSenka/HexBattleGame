using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public abstract class SaveableScriptableObject : ScriptableObject
    {
        public abstract object JsonObjectToSerialize { get; }

        public string SceneName;

#if UNITY_EDITOR
        public static T Load<T>(string path) where T : SaveableScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
                return ScriptableObject.CreateInstance<T>();

            return asset;
        }

        public virtual void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = AssetDatabase.GetAssetPath(this);

            if (File.Exists(path) || string.IsNullOrEmpty(path))
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            else
            {
                AssetDatabase.CreateAsset(this, path);
            }

            SerializeJsonObject(path);
        }

        public virtual void SerializeJsonObject(string path)
        {
            var text = JsonConvert.SerializeObject(JsonObjectToSerialize, Formatting.Indented);
            var jsonObjPath = Regex.Replace(path, @"\.asset$", ".json", RegexOptions.IgnoreCase);
            File.WriteAllText(jsonObjPath, text);

            AssetDatabase.Refresh();
        }
#endif
    }
}
