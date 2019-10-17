using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUnitHexHighlights), true)]
    public class UnitHexHighlights : IUnitHexHighlights
    {
        private IEnumerable<PoolItem> m_MovementItems;
        private IEnumerable<PoolItem> m_AttackItems;
        private IEnumerable<PoolItem> m_AttackableUnitsHighlights;
        private IEnumerable<PoolItem> m_PathItems;

        private bool m_IsUnitSelected;
        private Unit m_SelectedUnit;

        private readonly IUnitSelectionManager UnitSelectionManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly IUserInputManager UserInputManager;
        private readonly IHexDatabase HexDatabase;
        private readonly IUnitAttackManager UnitAttackManager;
        public UnitHexHighlights(IUserInputManager UserInputManager, IHexHighlighter HexHighlighter, IUnitSelectionManager UnitSelectionManager,
            IHexDatabase HexDatabase, IUnitAttackManager UnitAttackManager)
        {
            this.UnitSelectionManager = UnitSelectionManager;
            this.HexHighlighter = HexHighlighter;
            this.UserInputManager = UserInputManager;
            this.HexDatabase = HexDatabase;
            this.UnitAttackManager = UnitAttackManager;

            UnitSelectionManager.UnitSelected += UnitSelected;
            UnitSelectionManager.UnitUnselected += UnitUnselected;

            UserInputManager.HexUnderMouseChanged += OnHexUnderMouseChanged;
        }

        private void OnHexUnderMouseChanged(HexCell hex)
        {
            if (m_IsUnitSelected && hex != default && UnitSelectionManager.Paths.ContainsKey(hex.Position))
            {
                var highlighter = UnitSelectionManager.CanLocalPlayerControlThisUnit(m_SelectedUnit) ? Highlighter.white_light : Highlighter.white_light;

                var path = UnitSelectionManager.Paths.CalculatePath(hex.Position);
                m_PathItems = HexHighlighter.PlaceHighlighters(path, highlighter, m_PathItems);
            }
            else
            {
                ReleaseItems(ref m_PathItems);
            }
        }

        private void UnitSelected(Unit unit)
        {
            // Highlight movement radius
            var movementCoverage = UnitSelectionManager.Paths.CoveredCells();
            var highlighter = UnitSelectionManager.CanLocalPlayerControlThisUnit(unit) ? Highlighter.blue : Highlighter.grey;
            m_MovementItems = HexHighlighter.PlaceHighlighters(movementCoverage, highlighter, m_MovementItems);

            // Highlight attack radius
            m_AttackItems = HexHighlighter.PlaceHighlighters(UnitAttackManager.AttackRadius, Highlighter.icon_attack_red, m_AttackItems);

            // If unit is in range of attack, highlight it in red
            var attackableUnits = HexDatabase.Selectables.Where(s => UnitAttackManager.AttackRadius.Contains(s.Cell)).Select(s => s.Cell);
            m_AttackableUnitsHighlights = HexHighlighter.PlaceHighlighters(attackableUnits, Highlighter.red, m_AttackableUnitsHighlights);

            ReleaseItems(ref m_PathItems);
            m_IsUnitSelected = true;
            m_SelectedUnit = unit;
        }

        private void UnitUnselected(Unit unit)
        {
            if (m_MovementItems == null)
                Debug.LogWarning("m_MovementItems were already released. Did UnitUnselected callback was fired for already unselected unit? " + unit);

            ReleaseItems(ref m_AttackItems);
            ReleaseItems(ref m_AttackableUnitsHighlights);
            ReleaseItems(ref m_MovementItems);
            ReleaseItems(ref m_PathItems);

            m_IsUnitSelected = false;
            m_SelectedUnit = null;
        }

        private void ReleaseItems(ref IEnumerable<PoolItem> items)
        {
            if (items != null)
            {
                items.Release();
                items = null;
            }
        }
    }
}
