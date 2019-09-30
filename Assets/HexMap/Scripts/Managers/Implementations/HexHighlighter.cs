using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    /// <summary>
    /// This class is supposed to be used to place highlighters on the map from the pools.
    /// This class does not act on its own from any event callbacks
    /// </summary>
    [RegisterDependency(typeof(IHexHighlighter), true)]
    public class HexHighlighter : IHexHighlighter
    {
        private readonly PublicReferences PrefabReferences;

        private ObjectPool m_PoolBlue;
        private ObjectPool m_PoolRed;
        private ObjectPool m_PoolSelection;
        private ObjectPool m_PoolHover;

        public HexHighlighter(PublicReferences PrefabReferences)
        {
            this.PrefabReferences = PrefabReferences;
        }

        public void Start()
        {
            m_PoolBlue = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.BlueHighlightPrefab, 0);
            m_PoolRed = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.RedHighlightPrefab, 0);
            m_PoolSelection = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.SelectionHighlightPrefab, 0);
            m_PoolHover = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.HoverHighlightPrefab, 0);
        }

        public PoolItem PlaceHighlighter(HexCell hexCell, Highlighter highlighter, PoolItem reusePoolItem = null)
        {
            // If reusable item is null or already released, create a new one
            if (reusePoolItem == null || !reusePoolItem.IsReserved)
                reusePoolItem = ReserveItems(highlighter, 1).First();

            reusePoolItem.GameObject.transform.position = hexCell.WorldPosition;
            return reusePoolItem;
        }

        private IEnumerable<PoolItem> ReserveItems(Highlighter highlighter, int amount)
        {
            switch (highlighter)
            {
                case Highlighter.Blue:
                    return m_PoolBlue.ReserveItems(amount);
                case Highlighter.Red:
                    return m_PoolRed.ReserveItems(amount);
                case Highlighter.Hover:
                    return m_PoolHover.ReserveItems(amount);
                case Highlighter.Select:
                    return m_PoolSelection.ReserveItems(amount);
                default:
                    throw new ArgumentException("Unknown highlighter type: " + highlighter);
            }
        }
    }

    public enum Highlighter
    {
        Blue, Red, Hover, Select
    }
}
