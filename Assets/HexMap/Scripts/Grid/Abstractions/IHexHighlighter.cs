using System.Collections.Generic;

namespace Assets
{
    public interface IHexHighlighter
    {
        PoolItem PlaceHighlighter(HexCell hexCell, Highlighter highlighter, PoolItem reusePoolItem = null);
        IEnumerable<PoolItem> PlaceHighlighters(IEnumerable<HexCell> cells, Highlighter highlighter, IEnumerable<PoolItem> reuseTheseItems = null);

        void Start();
    }
}
