using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IHexDatabase), true)]
    public class HexDatabase : IHexDatabase
    {
        private IDictionary<int2, HexCell> m_CellMap;
        private IList<Selectable> m_SelectableMap;

        public HexDatabase()
        {
            m_CellMap = new Dictionary<int2, HexCell>();
            m_SelectableMap = new List<Selectable>();
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

        public void UpdateHexCell(HexCell hex)
        {
            m_CellMap[hex.Position] = hex;
        }

        public Selectable GetSelectable(int2 pos)
        {
            return m_SelectableMap.FirstOrDefault(e => e.Cell == pos);
        }

        public void AddNewSelectable(Selectable obj)
        {
            if (m_SelectableMap.Contains(obj))
            {
                Debug.LogWarning($"Selectable is already in HexDatabase: {obj}");
                return;
            }

            m_SelectableMap.Add(obj);
        }

        public Unit[] GetUnitsForTeam(ITeam Team)
        {
            return m_SelectableMap
                .Where(sel => sel.Team == Team.TeamID && sel is Unit)
                .Select(sel => (Unit)sel).ToArray();
        }
    }
}
