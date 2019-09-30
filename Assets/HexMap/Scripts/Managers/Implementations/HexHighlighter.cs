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
                reusePoolItem = ReserveItem(highlighter);

            reusePoolItem.GameObject.transform.position = hexCell.WorldPosition;
            return reusePoolItem;
        }

        public IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<HexCell> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null)
        {
            var itemsNeeded = cells.Count();

            // If reusable items enumerable is null, doesn't match number needed or any items were released, reserve new array
            if (reuseTheseItems == null || itemsNeeded != reuseTheseItems.Count() || !reuseTheseItems.First().IsReserved)
            {
                reuseTheseItems?.Release();
                reuseTheseItems = ReserveItems(highlighter, itemsNeeded);
            }

            // for each item assing one cell from the list
            foreach (var (item, cell) in reuseTheseItems.Zip(cells, (item, cell) => (item, cell)))
                item.GameObject.transform.position = cell.WorldPosition;

            return reuseTheseItems;
        }

        public IEnumerable<PoolItem> PlaceHighlightersAround(HexCell hexCell, Highlighter highlighter, int minRange, int maxRange, IEnumerable<PoolItem> reuseTheseItems = null)
        {
            var itemsNeeded = CalculateAmountOfItemsNeeded(minRange, maxRange);

            // If reusable items enumerable is null, doesn't match number needed or any items were released, reserve new array
            if (reuseTheseItems == null || itemsNeeded != reuseTheseItems.Count() || !reuseTheseItems.First().IsReserved)
            {
                reuseTheseItems?.Release();
                reuseTheseItems = ReserveItems(highlighter, itemsNeeded);
            }

            foreach (var item in reuseTheseItems)
                item.GameObject.transform.position = hexCell.WorldPosition; //DEFINITELY WILL NOT WORK

            return reuseTheseItems;
        }

        private int CalculateAmountOfItemsNeeded(int minRange, int maxRange)
        {
            return 7;
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
