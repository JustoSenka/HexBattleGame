using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class HexDatabaseData : SaveableScriptableObject
    {
        public List<HexTypeElement> HexTypeData = new List<HexTypeElement>();

        public List<Selectable> SelectableData = new List<Selectable>();
        public List<Movable> MovableData = new List<Movable>();
        public List<Unit> UnitData = new List<Unit>();

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
