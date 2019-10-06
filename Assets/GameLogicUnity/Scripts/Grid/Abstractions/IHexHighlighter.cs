using System.Collections.Generic;

namespace Assets
{
    public interface IHexHighlighter
    {
        PoolItem PlaceHighlighter(int2 cell, Highlighter highlighter, PoolItem reusePoolItem = null);
        PoolItem PlaceHighlighter(HexCell hexCell, Highlighter highlighter, PoolItem reusePoolItem = null);

        IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<int2> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null);
        IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<HexCell> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null);

        void Start();
    }
}
