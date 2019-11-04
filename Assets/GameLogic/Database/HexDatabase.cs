using Assets.GameLogic.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IHexDatabase), true)]
    public class HexDatabase : IHexDatabase
    {
        public event Action<Map> BeforeMapUnload;
        public event Action<Map> AfterMapLoaded;

        public event Action<Selectable> SelectableAdded;
        public event Action<Selectable> SelectableRemoved;

        public IEnumerable<Selectable> Selectables => m_SelectableMap.AsEnumerable();

        private Map m_CurrentlyLoadedMap;

        private IDictionary<int2, HexCell> m_CellMap;
        private IList<Selectable> m_SelectableMap;

        public HexDatabase()
        {
            m_CellMap = new Dictionary<int2, HexCell>();
            m_SelectableMap = new List<Selectable>();
        }

        public void UnloadMap()
        {
            if (m_CurrentlyLoadedMap != null)
                BeforeMapUnload?.Invoke(m_CurrentlyLoadedMap);

            m_CurrentlyLoadedMap = null;
            m_CellMap.Clear();
            m_SelectableMap.Clear();
        }

        public void LoadMap(Map map)
        {
            UnloadMap();
            LoadMapInternal(map);

            m_CurrentlyLoadedMap = map;
            AfterMapLoaded?.Invoke(m_CurrentlyLoadedMap);
        }

        private void LoadMapInternal(Map map)
        {
            foreach (var el in map.HexTypeData)
            {
                var newCell = new HexCell(el.Hex, el.HexType);
                UpdateHexCell(newCell);
            }

            foreach (var el in map.SelectableData)
                AddNewSelectable(el.Clone());

            foreach (var el in map.MovableData)
                AddNewSelectable(el.Clone());

            foreach (var el in map.UnitData)
                AddNewSelectable(el.Clone());
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

            var hex = GetHex(obj.Cell);
            hex.Type = HexType.Unit;
            UpdateHexCell(hex);
            SelectableAdded?.Invoke(obj);
        }

        public void RemoveSelectable(Selectable obj)
        {
            if (!m_SelectableMap.Contains(obj))
            {
                Debug.LogWarning($"Selectable was not found when in HexDatabase when trying to remove it: {obj}");
                return;
            }

            m_SelectableMap.Remove(obj);

            var hex = GetHex(obj.Cell);
            hex.ResetHexType();
            UpdateHexCell(hex);
            SelectableRemoved?.Invoke(obj);
        }

        // Not used
        public Unit[] GetUnitsForTeam(int team)
        {
            return m_SelectableMap
                .Where(sel => sel.Team == team && sel is Unit)
                .Select(sel => (Unit)sel).ToArray();
        }
    }
}
