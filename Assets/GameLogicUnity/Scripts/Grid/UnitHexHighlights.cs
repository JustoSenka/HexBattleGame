using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IUnitHexHighlights), true)]
    public class UnitHexHighlights : IUnitHexHighlights
    {
        private readonly IUnitMovementManager UnitMovementManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IUserInputManager MouseManager;

        private IEnumerable<PoolItem> m_MovementItems;
        private IEnumerable<PoolItem> m_PathItems;

        private bool m_IsUnitSelected;

        public UnitHexHighlights(IUserInputManager MouseManager, IHexHighlighter HexHighlighter, IUnitMovementManager UnitMovementManager)
        {
            this.UnitMovementManager = UnitMovementManager;
            this.HexHighlighter = HexHighlighter;
            this.MouseManager = MouseManager;

            UnitMovementManager.UnitSelected += UnitSelected;
            UnitMovementManager.UnitUnselected += UnitUnselected;

            MouseManager.HexUnderMouseChanged += OnHexUnderMouseChanged;
        }

        private void OnHexUnderMouseChanged(HexCell hex)
        {
            if (m_IsUnitSelected && hex != default && UnitMovementManager.Paths.ContainsKey(hex.Position))
            {
                var path = UnitMovementManager.Paths.CalculatePath(hex.Position);
                m_PathItems = HexHighlighter.PlaceHighlighters(path, Highlighter.Red, m_PathItems);
            }
            else
            {
                ReleasePathItems();
            }
        }

        public void Update()
        {

        }

        private void UnitSelected(Unit unit)
        {
            var coverage = UnitMovementManager.Paths.CoveredCells();
            m_MovementItems = HexHighlighter.PlaceHighlighters(coverage, Highlighter.Blue, m_MovementItems);

            ReleasePathItems();
            m_IsUnitSelected = true;
        }

        private void UnitUnselected(Unit unit)
        {
            m_MovementItems.Release();
            m_MovementItems = null;

            ReleasePathItems();
            m_IsUnitSelected = false;
        }

        private void ReleasePathItems()
        {
            if (m_PathItems != null)
            {
                m_PathItems.Release();
                m_PathItems = null;
            }
        }
    }
}
