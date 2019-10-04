using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(UnitHexHighlights), true)]
    public class UnitHexHighlights
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IHexPathfinder HexPathfinder;

        private IEnumerable<PoolItem> m_MovementItems;

        public UnitHexHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter, IHexPathfinder HexPathfinder)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;
            this.HexPathfinder = HexPathfinder;

            MouseManager.SelectableSelected += OnSelectableClicked;
            MouseManager.SelectableUnselected += OnSelectableUnselected;
        }

        private void OnSelectableClicked(Selectable obj)
        {
            var unit = obj as Unit;
            if (unit != null)
            {
                var pathDict = HexPathfinder.FindAllPaths(unit.HexCell.Position, HexType.Empty, unit.Movement);
                var coverage = pathDict.CoveredCells().Select(pos => new HexCell(pos));
                m_MovementItems = HexHighlighter.PlaceHighlighters(coverage, Highlighter.Blue, m_MovementItems);
            }
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            m_MovementItems.Release();
            m_MovementItems = null;
        }


        public void Update()
        {

        }
    }
}
