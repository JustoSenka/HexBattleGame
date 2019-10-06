using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IHexDatabase), true)]
    public class HexDatabase : IHexDatabase
    {
        public IDictionary<int2, HexCell> m_CellMap;
        public IDictionary<int2, Selectable> m_SelectableMap;
        public IDictionary<int2, Movable> m_MovableMap;
        public IDictionary<int2, Unit> m_UnitMap;

        public HexDatabase()
        {
            m_CellMap = new Dictionary<int2, HexCell>();
            m_SelectableMap = new Dictionary<int2, Selectable>();
        }

        public HexCell GetHex(int2 pos)
        {
            if (!m_CellMap.TryGetValue(pos, out HexCell hexCell))
            {
                hexCell = new HexCell(pos);
                m_CellMap[pos] = hexCell;
            }

            return hexCell;
        }

        public void UpdateHex(HexCell hex) => m_CellMap[hex.Position] = hex;

        public Selectable GetSelectable(int2 pos)
        {
            m_SelectableMap.TryGetValue(pos, out Selectable sel);
            return sel;
        }

        public void UpdateSelectable(Selectable obj) => m_SelectableMap[obj.Cell] = obj;
    }
}
