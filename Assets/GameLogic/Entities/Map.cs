using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class Map
    {
        public string Name;

        public List<HexTypeElement> HexTypeData = new List<HexTypeElement>();

        public List<Selectable> SelectableData = new List<Selectable>();
        public List<Movable> MovableData = new List<Movable>();
        public List<Unit> UnitData = new List<Unit>();

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

#if UNITY_EDITOR
        public void ClearMapData()
        {
            HexTypeData.Clear();
            SelectableData.Clear();
            MovableData.Clear();
            UnitData.Clear();
        }
#endif
    }
}
