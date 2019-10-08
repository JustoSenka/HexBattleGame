using System;
using System.Collections.Generic;

namespace Assets
{
    /// <summary>
    /// Has callbacks when unit is selected, unselected, being moved to different position. 
    /// Also contains PathfinderDictionary for currently selected unit
    /// </summary>
    /// <remarks>
    /// This class acts on its own based on SelectionManager callbacks.
    /// It should not be used as a tool.
    /// </remarks>
    [RegisterDependency(typeof(IUnitMovementManager), true)]
    public class UnitMovementManager : IUnitMovementManager
    {
        public PathfinderDictionary Paths { get; private set; }

        public event Action<Unit> UnitSelected;
        public event Action<Unit> UnitUnselected;

        public event Action<Action, Unit, IEnumerable<int2>> UnitPositionChange;

        private Unit m_SelectedUnit;
        private Unit m_UnitWhichIsBeingMovedRightNow;

        private readonly ISelectionManager SelectionManager;
        private readonly IHexPathfinder HexPathfinder;
        private readonly IHexDatabase HexDatabase;
        public UnitMovementManager(ISelectionManager SelectionManager, IHexPathfinder HexPathfinder, IHexDatabase HexDatabase)
        {
            this.SelectionManager = SelectionManager;
            this.HexPathfinder = HexPathfinder;
            this.HexDatabase = HexDatabase;

            SelectionManager.HexClicked += OnHexClicked;

            SelectionManager.SelectableSelected += OnSelectableSelected;
            SelectionManager.SelectableUnselected += OnSelectableUnselected;
        }

        // Callbacks ---

        private void OnHexClicked(HexCell hex)
        {
            // Intercept SelectionManager unselecting unit when user is trying to move an unit
            if (m_SelectedUnit != default && hex.IsValid && Paths.ContainsKey(hex.Position))
            {
                SelectionManager.DoNotAllowOtherSystemsToChangeSelection = true;
                SelectionManager.HexSelectionAborted += OnHexSelectionAborted;

                MoveSelectedUnitTo(hex);
            }
        }

        private void OnHexSelectionAborted(HexCell hex)
        {
            SelectionManager.HexSelectionAborted -= OnHexSelectionAborted;
            SelectionManager.DoNotAllowOtherSystemsToChangeSelection = false;

            // We need to first unselect old one first. If we select already selected item, it will not fire any callbacks
            m_UnitWhichIsBeingMovedRightNow = m_SelectedUnit;
            SelectionManager.SelectSelectable(default);
        }

        private void UnitPositionUpdatedFromUI()
        {
            SelectionManager.SelectSelectable(m_UnitWhichIsBeingMovedRightNow);
            m_UnitWhichIsBeingMovedRightNow = default;
        }

        private void OnSelectableSelected(Selectable obj)
        {
            if (obj is Unit unit)
                SelectUnit(unit);
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            if (obj is Unit unit)
                UnselectUnit(unit);
        }

        // Private ---

        private void MoveSelectedUnitTo(HexCell hexTo)
        {
            var path = Paths.CalculatePath(hexTo.Position);
            UnitPositionChange?.Invoke(UnitPositionUpdatedFromUI, m_SelectedUnit, path);

            var hexFrom = HexDatabase.GetHex(m_SelectedUnit.Cell);
            m_SelectedUnit.Cell = hexTo.Position;

            hexFrom.Type = HexType.Empty;
            hexTo.Type = HexType.Unit;

            HexDatabase.UpdateHexCell(hexFrom);
            HexDatabase.UpdateHexCell(hexTo);
        }

        private void SelectUnit(Unit unit)
        {
            m_SelectedUnit = unit;
            Paths = HexPathfinder.FindAllPaths(unit.Cell, HexType.Empty, unit.Movement);
            UnitSelected?.Invoke(unit);
        }

        private void UnselectUnit(Unit unit)
        {
            m_SelectedUnit = default;
            UnitUnselected?.Invoke(unit);
        }
    }
}
