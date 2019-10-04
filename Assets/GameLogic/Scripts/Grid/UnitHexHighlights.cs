using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(IUnitHexHighlights), true)]
    public class UnitHexHighlights : IUnitHexHighlights
    {
        private readonly IUnitMovementManager UnitMovementManager;
        private readonly IHexHighlighter HexHighlighter;
        private IEnumerable<PoolItem> m_MovementItems;

        public UnitHexHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter, IUnitMovementManager UnitMovementManager)
        {
            this.UnitMovementManager = UnitMovementManager;
            this.HexHighlighter = HexHighlighter;

            UnitMovementManager.UnitSelected += UnitSelected;
            UnitMovementManager.UnitUnselected += UnitUnselected;
        }

        private void UnitSelected(Unit unit)
        {
            var coverage = UnitMovementManager.Paths.CoveredCells().Select(pos => new HexCell(pos));
            m_MovementItems = HexHighlighter.PlaceHighlighters(coverage, Highlighter.Blue, m_MovementItems);
        }

        private void UnitUnselected(Unit unit)
        {
            m_MovementItems.Release();
            m_MovementItems = null;
        }

        public void Update()
        {

        }
    }
}
