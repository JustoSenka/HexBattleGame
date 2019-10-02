using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(HexDebugHighlights), true)]
    public class HexDebugHighlights
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;

        private PoolItem m_Item;

        public HexDebugHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.HexSelected += OnHexSelected;
            MouseManager.HexUnselected += OnHexUnselected;
        }


        private void OnHexSelected(HexCell hex)
        {
            var pos = HexUtility.FindIntersectingHexCell(new int2(-4, 2), hex.Position);
            var middleHex = new HexCell(pos);
            m_Item = HexHighlighter.PlaceHighlighter(middleHex, Highlighter.Blue, m_Item);
        }

        private void OnHexUnselected(HexCell hex)
        {
            m_Item?.Release();
            m_Item = null;
        }
    }
}
