using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// This class is supposed to be used to place highlighters on the map from the pools.
    /// This class does not act on its own from any event callbacks
    /// </summary>
    [RegisterDependency(typeof(IHexHighlighter), true)]
    public class HexHighlighter : IHexHighlighter
    {
        [Dependency(typeof(PublicReferences))]
        private PublicReferences PublicReferences;

        private ObjectPool m_PoolBlue;
        private ObjectPool m_PoolRed;
        private ObjectPool m_PoolSelection;
        private ObjectPool m_PoolHover;

        public HexHighlighter()
        {

        }

        public void Start()
        {
            var parent = new GameObject("Pool");

            // Public References might not be created when running tests.
            // In that case just pretend to create some empty game objects. For testing it is enough
            // Print a warning in case we are not running test so someone can notice
            if (PublicReferences == null)
            {
                Debug.LogWarning(this.GetType() + ": PublicReferences == null");
                var go = new GameObject("Dummy");
                m_PoolBlue = new ObjectPool(parent, go, 0);
                m_PoolRed = new ObjectPool(parent, go, 0);
                m_PoolSelection = new ObjectPool(parent, go, 0);
                m_PoolHover = new ObjectPool(parent, go, 0);

                return;
            }

            m_PoolBlue = new ObjectPool(parent, PublicReferences.BlueHighlightPrefab, 0);
            m_PoolRed = new ObjectPool(parent, PublicReferences.RedHighlightPrefab, 0);
            m_PoolSelection = new ObjectPool(parent, PublicReferences.SelectionHighlightPrefab, 0);
            m_PoolHover = new ObjectPool(parent, PublicReferences.HoverHighlightPrefab, 0);
        }

        public PoolItem PlaceHighlighter(int2 cell, Highlighter highlighter, PoolItem reusePoolItem = null) => PlaceHighlighter(new HexCell(cell), highlighter, reusePoolItem);
        public PoolItem PlaceHighlighter(HexCell hexCell, Highlighter highlighter, PoolItem reusePoolItem = null)
        {
            // If reusable item is null or already released, create a new one
            if (reusePoolItem == null || !reusePoolItem.IsReserved)
                reusePoolItem = ReserveItem(highlighter);

            reusePoolItem.GameObject.transform.position = hexCell.WorldPosition;
            reusePoolItem.Cell = hexCell.Position;
            return reusePoolItem;
        }

        public IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<int2> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null) => PlaceHighlighters(cells.Select(c => new HexCell(c)), highlighter, reuseTheseItems);
        public IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<HexCell> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null)
        {
            var itemsNeeded = cells.Count();

            // If reusable items enumerable is null, doesn't match number needed or any items were released, reserve new array
            if (reuseTheseItems == null || itemsNeeded != reuseTheseItems.Count() || itemsNeeded == 0 || !reuseTheseItems.First().IsReserved)
            {
                reuseTheseItems?.Release();
                reuseTheseItems = ReserveItems(highlighter, itemsNeeded);
            }

            // for each item assing one cell from the list
            foreach (var (item, cell) in reuseTheseItems.Zip(cells, (item, cell) => (item, cell)))
            {
                item.GameObject.transform.position = cell.WorldPosition;
                item.Cell = cell.Position;
            }

            return reuseTheseItems;
        }

        private PoolItem ReserveItem(Highlighter highlighter)
        {
            switch (highlighter)
            {
                case Highlighter.Blue:
                    return m_PoolBlue.ReserveItem();
                case Highlighter.Red:
                    return m_PoolRed.ReserveItem();
                case Highlighter.Hover:
                    return m_PoolHover.ReserveItem();
                case Highlighter.Select:
                    return m_PoolSelection.ReserveItem();
                default:
                    throw new ArgumentException("Unknown highlighter type: " + highlighter);
            }
        }

        // Reserving item calls should always iterate the collection when call has been made to reserve all the pool items right away
        private IEnumerable<PoolItem> ReserveItems(Highlighter highlighter, int amount) => ReserveItemsEnumerable(highlighter, amount).ToArray();
        private IEnumerable<PoolItem> ReserveItemsEnumerable(Highlighter highlighter, int amount)
        {
            for (int i = 0; i < amount; i++)
                yield return ReserveItem(highlighter);
        }
    }

    public enum Highlighter
    {
        Blue, Red, Hover, Select
    }
}
