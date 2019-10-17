using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class SaveableScriptableObject : ScriptableObject
    {
        public string SceneName;

#if UNITY_EDITOR
        public static T Load<T>(string path) where T : SaveableScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
                return ScriptableObject.CreateInstance<T>();

            return asset;
        }

        public void Save(string path)
        {
            if (File.Exists(path))
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            else
            {
                AssetDatabase.CreateAsset(this, path);
            }
        }
#endif
    }
}
