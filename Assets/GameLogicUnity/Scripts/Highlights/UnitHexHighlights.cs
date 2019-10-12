using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUnitHexHighlights), true)]
    public class UnitHexHighlights : IUnitHexHighlights
    {
        private IEnumerable<PoolItem> m_MovementItems;
        private IEnumerable<PoolItem> m_PathItems;

        private bool m_IsUnitSelected;
        private Unit m_SelectedUnit;

        private readonly IUnitMovementManager UnitMovementManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IUserInputManager UserInputManager;
        private readonly ITurnManager TurnManager;
        public UnitHexHighlights(IUserInputManager UserInputManager, IHexHighlighter HexHighlighter, IUnitMovementManager UnitMovementManager, ITurnManager TurnManager)
        {
            this.UnitMovementManager = UnitMovementManager;
            this.HexHighlighter = HexHighlighter;
            this.UserInputManager = UserInputManager;
            this.TurnManager = TurnManager;

            UnitMovementManager.UnitSelected += UnitSelected;
            UnitMovementManager.UnitUnselected += UnitUnselected;

            UserInputManager.HexUnderMouseChanged += OnHexUnderMouseChanged;
        }

        private void OnHexUnderMouseChanged(HexCell hex)
        {
            if (m_IsUnitSelected && hex != default && UnitMovementManager.Paths.ContainsKey(hex.Position))
            {
                var highlighter = TurnManager.CurrentTurnOwner == m_SelectedUnit ? Highlighter.white_light : Highlighter.white_light;

                var path = UnitMovementManager.Paths.CalculatePath(hex.Position);
                m_PathItems = HexHighlighter.PlaceHighlighters(path, highlighter, m_PathItems);
            }
            else
            {
                ReleasePathItems();
            }
        }

        private void UnitSelected(Unit unit)
        {
            var coverage = UnitMovementManager.Paths.CoveredCells();

            var highlighter = TurnManager.CurrentTurnOwner == unit ? Highlighter.blue : Highlighter.grey ;
            m_MovementItems = HexHighlighter.PlaceHighlighters(coverage, highlighter, m_MovementItems);

            ReleasePathItems();
            m_IsUnitSelected = true;
            m_SelectedUnit = unit;
        }

        private void UnitUnselected(Unit unit)
        {
            if (m_MovementItems != null)
            {
                m_MovementItems.Release();
                m_MovementItems = null;
            }
            else
                Debug.LogWarning("m_MovementItems were already released. Did UnitUnselected callback was fired for already unselected unit? " + unit);


            ReleasePathItems();
            m_IsUnitSelected = false;
            m_SelectedUnit = null;
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
