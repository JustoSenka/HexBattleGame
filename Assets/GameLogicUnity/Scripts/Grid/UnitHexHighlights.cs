using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUnitHexHighlights), true)]
    public class UnitHexHighlights : IUnitHexHighlights
    {
        private readonly IUnitMovementManager UnitMovementManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IUserInputManager UserInputManager;

        private IEnumerable<PoolItem> m_MovementItems;
        private IEnumerable<PoolItem> m_PathItems;

        private bool m_IsUnitSelected;

        public UnitHexHighlights(IUserInputManager UserInputManager, IHexHighlighter HexHighlighter, IUnitMovementManager UnitMovementManager)
        {
            this.UnitMovementManager = UnitMovementManager;
            this.HexHighlighter = HexHighlighter;
            this.UserInputManager = UserInputManager;

            UnitMovementManager.UnitSelected += UnitSelected;
            UnitMovementManager.UnitUnselected += UnitUnselected;

            UserInputManager.HexUnderMouseChanged += OnHexUnderMouseChanged;
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

        private void UnitSelected(Unit unit)
        {
            var coverage = UnitMovementManager.Paths.CoveredCells();
            m_MovementItems = HexHighlighter.PlaceHighlighters(coverage, Highlighter.Blue, m_MovementItems);

            ReleasePathItems();
            m_IsUnitSelected = true;
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
