using System;
using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(TurnOwnerHighlight), true)]
    public class TurnOwnerHighlight
    {
        private PoolItem m_HighlightItem;

        private readonly ITurnManager TurnManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IUnitMovementManager UnitMovementManager;
        public TurnOwnerHighlight(ITurnManager TurnManager, IHexHighlighter HexHighlighter, IUnitMovementManager UnitMovementManager)
        {
            this.HexHighlighter = HexHighlighter;
            this.TurnManager = TurnManager;
            this.UnitMovementManager = UnitMovementManager;

            TurnManager.TurnStarted += OnTurnStarted;
            TurnManager.TurnEnded += OnTurnEnded;

            UnitMovementManager.UnitPositionChange += OnUnitPositionChange;
            UnitMovementManager.UnitPositionChangeEnd += OnUnitPositionChangeEnd;
        }

        // Callbacks ---

        private void OnUnitPositionChange(Action<Unit> arg1, Unit arg2, IEnumerable<int2> arg3) => ReleaseHighlighterItem();
        private void OnUnitPositionChangeEnd(Unit obj) => PlaceHighlighterItem(obj.Cell);

        private void OnTurnStarted(Selectable obj) => PlaceHighlighterItem(obj.Cell);
        private void OnTurnEnded(Selectable obj) => ReleaseHighlighterItem();

        // Private ---

        private void PlaceHighlighterItem(int2 cell)
        {
            m_HighlightItem = HexHighlighter.PlaceHighlighter(cell, Highlighter.white, m_HighlightItem);
        }

        private void ReleaseHighlighterItem()
        {
            if (m_HighlightItem != null)
            {
                m_HighlightItem.Release();
                m_HighlightItem = null;
            }
        }
    }
}
