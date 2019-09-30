using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(UnitHexHighlights), true)]
    public class UnitHexHighlights
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;

        private IEnumerable<PoolItem> m_MovementItems;

        public UnitHexHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.SelectableSelected += OnSelectableClicked;
            MouseManager.SelectableUnselected += OnSelectableUnselected;
        }

        private void OnSelectableClicked(Selectable obj)
        {
            var unit = obj as Unit;
            if (unit != null)
            {
                var cellsToHighlight = HexCell.FindNeighbours(unit.HexCell.Position, unit.Movement).Select(pos => new HexCell(pos)).ToArray();
                m_MovementItems = HexHighlighter.PlaceHighlighters(cellsToHighlight, Highlighter.Blue, m_MovementItems);
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
