using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class HexDatabaseData : ScriptableObject
    {
        public string SceneName;

        public List<HexTypeElement> HexTypeData = new List<HexTypeElement>();

        public List<Selectable> SelectableData = new List<Selectable>();
        public List<Movable> MovableData = new List<Movable>();
        public List<Unit> UnitData = new List<Unit>();

        public static HexDatabaseData Load(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<HexDatabaseData>(path);
            if (asset == null)
                return ScriptableObject.CreateInstance<HexDatabaseData>();

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

        public void ClearAllData()
        {
            HexTypeData.Clear();
            SelectableData.Clear();
            MovableData.Clear();
            UnitData.Clear();
        }

        // Those structs are here because unity apparently cannot serialize Dictionaries or tuples or generic structs......

        [Serializable]
        public struct HexTypeElement
        {
            public int2 Hex;
            public HexType HexType;

            public HexTypeElement(int2 hex, HexType hexType)
            {
                Hex = hex;
                this.HexType = hexType;
            }
        }
    }
}
