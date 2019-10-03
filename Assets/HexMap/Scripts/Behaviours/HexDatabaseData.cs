using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class HexDatabaseData : ScriptableObject
    {
        public string SceneName;

        private IDictionary<int2, HexType> m_Data;
        public IDictionary<int2, HexType> Data
        {
            get
            {
                if (m_Data == default)
                {
                    m_Data = new Dictionary<int2, HexType>();
                    if (ListData != null)
                    {
                        foreach (var el in ListData)
                            m_Data[el.Hex] = el.HexType;
                    }
                }

                return m_Data;
            }
        }

        [SerializeField]
        private ListElement[] ListData;

        public void Save(string path)
        {
            ListData = Data.Select(k => new ListElement(k.Key, k.Value)).ToArray();
            AssetDatabase.CreateAsset(this, path);
        }

        [Serializable]
        private struct ListElement
        {
            public int2 Hex;
            public HexType HexType;

            public ListElement(int2 hex, HexType hexType)
            {
                Hex = hex;
                HexType = hexType;
            }
        }
    }
}
